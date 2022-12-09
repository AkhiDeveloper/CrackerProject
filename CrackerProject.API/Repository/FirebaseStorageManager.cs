using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;
using CrackerProject.API.Settings;
using Firebase.Auth;
using Firebase.Storage;
using ServiceStack.Messaging;
using Setting = CrackerProject.API.Settings;

namespace CrackerProject.API.Repository
{
    public class FirebaseStorageManager : IStorageManager
    {
        private StorageDirectory _currentdir;
        private readonly FirebaseStorage _storage;
        private readonly string _masterfolder = "Main";

        public FirebaseStorageManager(Setting.FirebaseConfig config)
        {
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(config.ApiKey));
            var a = auth.SignInWithEmailAndPasswordAsync(config.EmailId, config.Password).Result;
            var cancellation = new CancellationTokenSource();
            _storage = new FirebaseStorage(config.StorageBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                });
        }

        private FirebaseStorageReference GetFirebaseStorageRefrence(StorageDirectory? directory = null, FirebaseStorage? storage = null)
        {
            if(storage == null)
            {
                storage = _storage;
            }
            if(directory == null)
            {
                directory = new StorageDirectory(_masterfolder);
            }
            FirebaseStorageReference storageReference;
            if(directory.GetParentDirectory() != null)
            {
                storageReference = storage.Child(directory.Name);
                return storageReference;
            }
            storageReference = GetFirebaseStorageRefrence(directory.GetParentDirectory()).Child(directory.Name);
            return storageReference;
        }

        public async Task<StorageDirectory> GetActiveDirectory()
        {
            return _currentdir;
        }

        public Task ChangeActiveDirectory(StorageDirectory directory)
        {
            _currentdir = directory;
            return Task.CompletedTask;
        }

        public async Task<StorageDirectory> UploadFile(Stream fileStream, CancellationToken cancellationToken, string? filename = null)
        {
            if (filename == null)
            {
                filename = "file" + "_" + DateTime.UtcNow.Ticks;
            }
            var storageRef = GetFirebaseStorageRefrence(_currentdir);
            var task = await storageRef
                .Child(filename)
                .PutAsync(fileStream, cancellationToken);
            if(task == null)
            {
                throw new Exception("Failed to Upload File.");
            }
            return _currentdir.Child(filename);
        }


        public Task<bool> IsFileExist(string filename)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadFile(string filename)
        {
            throw new NotImplementedException();
        }

        public Task MoveFile(string filename, StorageDirectory destination)
        {
            throw new NotImplementedException();
        }

        public Task ChangeFileName(string filename, string newfilename)
        {
            throw new NotImplementedException();
        }

        public Task CreateFolder(string foldername)
        {
            throw new NotImplementedException();
        }
    }
}
