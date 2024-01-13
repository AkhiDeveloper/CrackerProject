using BookManager.API.Models;

namespace BookManager.API.ServiceProvider
{
    public interface IQuestionTextConverter
    {
        bool TryParseRawText(string qsnText, string optText, out IEnumerable<Question> questions);
        bool TryParseJsonText(string qsnJson, out IEnumerable<Question> questions);
    }
}
