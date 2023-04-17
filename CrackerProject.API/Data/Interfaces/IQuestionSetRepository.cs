using DataModel = CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using System.Linq.Expressions;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.Data.Interfaces
{
    public interface IQuestionSetRepository : IRepository<QuestionSet, Guid>
    {
        //Section
        Task AddtoSection(Guid sectionid, QuestionSet questionset);
        Task MovetoSection(Guid question_id, Guid section_id);
        Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid);
        Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid, Expression<Func<Section, bool>> predicate);
        Task RemoveSectionQuestionSets(Guid sectionid, DeleteType deleteType);
        Task<bool> IsExistInSection(Guid sectionid, Guid questionset_id);


    }
}
