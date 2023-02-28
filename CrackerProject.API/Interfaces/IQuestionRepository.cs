using DataModel = CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using System.Linq.Expressions;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.Interfaces
{
    public interface IQuestionRepository : IRepository<Question, Guid>
    {
        //Relation to QuestionSet
        Task AddtoQuestionSet(Guid questionsetId, Question question);
        Task MovetoQuestionSet(Guid questionId, Guid targetquestionsetId);
        Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId);
        Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId, Expression<Func<Question, bool>> predicate);
        Task<bool> IsExistInQuestionSet(Guid questionsetId, Guid questionId);

        //Chapter
        Task AssignSectionQuestionstoChapter(Guid chapter_id, Guid section_id);
        Task<IEnumerable<Question>> GetChapterQuestions(Guid chapter_id);
        Task<IEnumerable<Question>> GetChapterQuestions(Guid Chapter_id, Expression<Func<Question, bool>> predicate);
        Task<IEnumerable<Guid>> GetChapterQuestionIds(Guid chapter_id);
        Task UnAssignSectionQuestionfromChapter(Guid chapter_id, Guid section_id);
    }
}
