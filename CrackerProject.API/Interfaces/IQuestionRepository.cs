using CrackerProject.API.Model;
using System.Linq.Expressions;

namespace CrackerProject.API.Interfaces
{
    public interface IQuestionRepository : IRepository<Question, DataModels.Question, Guid>
    {
        //Relation to QuestionSet
        Task AddtoQuestionSet(Guid questionsetId, Question question);
        Task MovetoQuestionSet(Guid questionId, Guid targetquestionsetId);
        Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId);
        Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId, Expression<Func<Question, bool>> predicate);
        Task<bool> IsExistInQuestionSet(Guid questionsetId, Guid questionId);
    }
}
