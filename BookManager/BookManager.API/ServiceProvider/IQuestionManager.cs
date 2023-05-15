using BookManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.API.ServiceProvider
{
    public interface IQuestionManager
    {
        public int GetTotalNumberOfSet(Guid chapterId);
        public Task<IEnumerable<Question>> GetAllQuestions(Guid chapterId);
        public Task<IEnumerable<Question>> GetAllQuestionOfSet(Guid chapterId, int SetSN);
        public Task<IEnumerable<Question>> GetAllQuestionOfLastSetAsync(Guid chapterId);
        public Question GetQuestion(Guid chapterId);
        Task CreateQuestion(Guid chapterId, int set_no, Question question);

        Task CreateQuestionAtLastSet(Guid chapterId, Question question);

        Task ChangeImage(Guid questionId, byte[] image);

        Task ChangeOption(Guid questionId, int OptSN, Option option);

        Task ChangeOptions(Guid questionId, IEnumerable<Option> options);

        Task ChangeSN(Guid questionId, int SN);

        Task ChangeText(Guid questionId, string text);

        Task DeleteQuestion(Guid questionId);

        Task DeleteQuestionSet(Guid chapterId, int SN);
    }
}
