using BookManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.API.ServiceProvider
{
    public interface IQuestionManager
    {
        Task<int> GetTotalNumberOfSet(Guid chapterId);

        Task<IEnumerable<Question>> GetAllQuestions(Guid chapterId);

        Task<IEnumerable<Question>> GetAllQuestionOfSet(Guid chapterId, int SetSN);

        Task<IEnumerable<Question>> GetAllQuestionOfLastSet(Guid chapterId);

        Task<Question> GetQuestion(Guid questionId);

        Task<string> GetQuestionImageUri(Guid questionId);

        Task<string> GetOptionImageUri(Guid questionId, int opt_sn);

        Task CreateQuestion(Guid chapterId, int set_no, Question question);

        Task CreateQuestionAtLastSet(Guid chapterId, Question question);

        Task ChangeImage(Guid questionId, Stream image);

        Task ChangeOption(Guid questionId, int OptSN, Option option);

        Task ChangeOptionImage(Guid questionId, int opt_sn, Stream image);

        Task ChangeOptions(Guid questionId, IEnumerable<Option> options);

        Task ChangeSN(Guid questionId, int SN);

        Task ChangeText(Guid questionId, string text);

        Task DeleteQuestion(Guid questionId);

        Task DeleteQuestionSet(Guid chapterId, int SN);

    }
}
