namespace BookManager.API.ServiceProvider
{
    public class ImgurFileStorage
        : IFileStorage
    {
        public ImgurFileStorage(string clientID, string clientSecret)
        {

        }
        public Task DeleteFile(string fileName, string folderPath = "")
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadFile(string fileName, string folderPath = "")
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task UploadFile(string fileName, Stream stream, string folderPath = "")
        {
            throw new NotImplementedException();
        }
    }
}
