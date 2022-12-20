namespace CrackerProject.Library.Extractor
{
    public class Question
    {
        public int QuestionNumber { get; set; }
        public string Body { get; set; } = string.Empty;
        public IList<OptionSet> OptionSets { get; set; } = new List<OptionSet>();
        public string Hint { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
    }
}