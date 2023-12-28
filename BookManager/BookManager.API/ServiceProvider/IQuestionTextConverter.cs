using BookManager.API.Models;

namespace BookManager.API.ServiceProvider
{
    public interface IQuestionTextConverter
    {
        bool TryParse(string qsnText, string optText, out IEnumerable<Question> questions);
    }
}
