using BookManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.API.ServiceProvider
{
    public interface IQuestionManager
    {
        Task<int> GetTotalNumberOfSet(Guid chapterId);

        Task<IEnumerable<Question>> GetAllQuestions(Guid chapterId);

        Task<IEnumerable<Question>> GetAllQuestionOfSet(Guid chapterId, int SetSN);

        Task<IEnumerable<Question>> GetAllQuestionOfLastSetAsync(Guid chapterId);

        Question GetQuestion(Guid chapterId);

        Task CreateQuestion(Guid chapterId, int set_no, Question question);

        Task CreateQuestionAtLastSet(Guid chapterId, Question question);

        Task ChangeImage(Guid questionId, byte[] image);

        Task ChangeOption(Guid questionId, int OptSN, Option option);

        Task ChangeOptionImage(Guid questionId, byte[] image);

        Task ChangeOptions(Guid questionId, IEnumerable<Option> options);

        Task ChangeSN(Guid questionId, int SN);

        Task ChangeText(Guid questionId, string text);

        Task DeleteQuestion(Guid questionId);

        Task DeleteQuestionSet(Guid chapterId, int SN);
    }
}
