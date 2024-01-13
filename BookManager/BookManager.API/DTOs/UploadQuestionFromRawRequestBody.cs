namespace BookManager.API.DTOs
{
    public class UploadQuestionFromRawFileRequestBody
    {
        public IFormFile QuestionRawText { get; set; }
        public IFormFile CorrectQuestionRawText { get; set; }
    }
}
