using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;

namespace CrackerProject.API.Repository
{
    public class QuestionSetRepository : BaseRepository<QuestionSet>, IQuestionSetRepository
    {
        public QuestionSetRepository(IMongoContext context) : base(context)
        {
        }
    }
}
