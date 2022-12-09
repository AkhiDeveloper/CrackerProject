using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;
using System.Linq.Expressions;

namespace CrackerProject.API.Repository
{
    public class QuestionSetRepository : BaseRepository<QuestionSet, DataModels.QuestionSet,Guid>, IQuestionSetRepository
    {
        public QuestionSetRepository(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public Task AddtoSection(Guid sectionid, QuestionSet questionset)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid, Expression<Func<Section, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistInSection(Guid sectionid, Guid questionset_id)
        {
            throw new NotImplementedException();
        }

        public Task MovetoSection(Guid question_id, Guid section_id)
        {
            throw new NotImplementedException();
        }
    }
}
