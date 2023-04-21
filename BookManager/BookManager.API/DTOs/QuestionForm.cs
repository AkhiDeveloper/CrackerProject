namespace BookManager.API.DTOs
{
    public class QuestionForm
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string Text { get; set; }
        public string ImageUri { get; set; } = string.Empty;
        public IList<OptionForm> Options { get; set; } = new List<OptionForm>();
    }
}
