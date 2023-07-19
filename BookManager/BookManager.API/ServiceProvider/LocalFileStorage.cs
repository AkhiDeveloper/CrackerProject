namespace BookManager.API.ServiceProvider
{
    public class LocalFileStorage
        : IFileStorage, IDisposable
    {
        private readonly string _basePath;

        public LocalFileStorage(string basePath="Files")
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            _basePath = basePath;
        }

        public Task DeleteFile(string fileName, string folderPath = "")
        {
            var targetFolderPath = Path.Combine(_basePath, folderPath);
            if (!Directory.Exists(targetFolderPath))
            {
                throw new Exception("Folder not found!");
            }
            var targetFilePath = Path.Combine(targetFolderPath, fileName);
            if (!File.Exists(targetFilePath))
            {
                throw new Exception("File not found");
            }
            File.Delete(targetFilePath);
            return Task.CompletedTask;
        }

        public Task DeleteFile(string filePath)
        {
            var targetFilePath = Path.Combine(_basePath, filePath);
            if (!File.Exists(targetFilePath))
            {
                throw new Exception("File not found");
            }
            File.Delete(targetFilePath);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public async Task<Stream> DownloadFile(string fileName, string folderPath = "")
        {
            var targetFolderPath = Path.Combine(_basePath, folderPath);
            if (!Directory.Exists(targetFolderPath))
            {
                throw new Exception("Folder not found!");
            }
            var targetFilePath = Path.Combine(targetFolderPath, fileName);
            if (!File.Exists(targetFilePath))
            {
                throw new Exception("File not found");
            }
            Stream fileStream = new FileStream(targetFilePath, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        public async Task<Stream> DownloadFile(string filePath)
        {
            var targetFilePath = Path.Combine(_basePath, filePath);
            if (!File.Exists(targetFilePath))
            {
                throw new Exception("File not found");
            }
            Stream fileStream = new FileStream(targetFilePath, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        public async Task UploadFile(string fileName, Stream stream, string folderPath = "")
        {
            var targetFolderPath = Path.Combine(_basePath, folderPath);
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }
            var targetFilePath = Path.Combine(targetFolderPath, fileName);
            Stream fileStream = new FileStream(targetFilePath, FileMode.CreateNew, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
            await fileStream.DisposeAsync();
        }
    }
}
