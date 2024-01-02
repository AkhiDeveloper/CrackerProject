namespace BookManager.API.Models
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string Text { get; set; }
        public Stream? Image { get; set; }
        public IList<OptionsSet> OptionsSets { get; set; }
    }
}
