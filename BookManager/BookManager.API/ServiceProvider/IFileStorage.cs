namespace BookManager.API.ServiceProvider
{
    public interface IFileStorage
    {
        Task UploadFile(string fileName, Stream stream, string folderPath = "");
        Task DeleteFile(string fileName, string folderPath="");
        Task DeleteFile(string filePath);
        Task<Stream> DownloadFile(string fileName, string folderPath = "");
        Task<Stream> DownloadFile(string filePath);
    }
}
