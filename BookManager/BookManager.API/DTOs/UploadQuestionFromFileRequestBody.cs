namespace BookManager.API.DTOs
{
    public class UploadQuestionFromFileRequestBody
    {
        public IFormFile QuestionRawText { get; set; }
        public IFormFile CorrectQuestionRawText { get; set; }
    }
}
