using CrackerProject.API.Model;
using System.Reflection.Metadata;

namespace CrackerProject.API.Data.Interfaces
{
    public interface IStorageManager
    {
        Task<StorageDirectory> GetActiveDirectory();
        Task ChangeActiveDirectory(StorageDirectory directory);
        Task<StorageDirectory> UploadFile(Stream fileStream, CancellationToken cancellationToken, string? filename = null);
        Task<bool> IsFileExist(string filename);
        Task DeleteFile(string filename);
        Task<byte[]> DownloadFile(string filename);
        Task MoveFile(string filename, StorageDirectory destination);
        Task ChangeFileName(string filename, string newfilename);
        Task CreateFolder(string foldername);

    }
}
