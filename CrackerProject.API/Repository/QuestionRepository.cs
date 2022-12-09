using CrackerProject.API.Model;
using CrackerProject.API.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace CrackerProject.API.Repository
{
    public class QuestionRepository : BaseRepository<Question,DataModels.Question,Guid>, IQuestionRepository
    {
        public QuestionRepository(IMongoContext context, IMapper mapper) : base(context,mapper)
        {
            
        }

        public Task AddtoQuestionSet(Guid questionsetId, Question question)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId, Expression<Func<Question, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistInQuestionSet(Guid questionsetId, Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task MovetoQuestionSet(Guid questionId, Guid targetquestionsetId)
        {
            throw new NotImplementedException();
        }
    }
}
