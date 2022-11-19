namespace CrackerProject.API.ViewModel
{
    public class ObjectiveQuestionForm : QuestionForm
    {
        public IList<OptionForm> Options { get; set; } = new List<OptionForm>();
    }
}
