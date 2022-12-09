using CrackerProject.API.Model;
using System.Linq.Expressions;

namespace CrackerProject.API.Interfaces
{
    public interface IQuestionSetRepository : IRepository<QuestionSet, DataModels.QuestionSet, Guid>
    {
        //Section
        Task AddtoSection(Guid sectionid, QuestionSet questionset);
        Task MovetoSection(Guid question_id, Guid section_id);
        Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid);
        Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid, Expression<Func<Section, bool>> predicate);
        Task<bool> IsExistInSection(Guid sectionid, Guid questionset_id);
    }
}
