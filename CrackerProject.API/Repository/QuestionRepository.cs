using CrackerProject.API.Model;
using CrackerProject.API.Interfaces;
using AutoMapper;
using System.Linq.Expressions;
using MongoDB.Driver;
using Humanizer;

namespace CrackerProject.API.Repository
{
    public class QuestionRepository : BaseRepository<Question,DataModels.Question,Guid>, IQuestionRepository
    {
        private readonly IMongoCollection<DataModels.Section> _sections;
        public QuestionRepository(IMongoContext context, IMapper mapper) : base(context,mapper)
        {
            _sections = Context.GetCollection<DataModels.Section>
                (nameof(DataModels.Section).Pluralize());
        }

        private async Task<DataModels.Section> GetParentSectionofQuestion(Guid question_id)
        {
            var parentsectioncursor = await _sections.FindAsync(s =>
            s.QuestionSets.Any(qs =>
            qs.Questions.Any(q =>
            q.id == question_id)));
            var parentsection = parentsectioncursor.FirstOrDefault();
            if(parentsection == null)
            {
                throw new NullReferenceException
                    ($"Question with id ={question_id} is not found.");
            }
            return parentsection;
        }

        private async Task<DataModels.Section> GetParentSectionofQuestionSet(Guid questionset_id)
        {
            var originsectioncursor = await _sections.FindAsync(x =>
            x.QuestionSets.Any(q =>
            q.Id == questionset_id));
            var originsection = originsectioncursor.FirstOrDefault();
            if (originsection == null)
            {
                throw new NullReferenceException
                    ($"Questionset with id ={questionset_id} is not found.");
            }
            return originsection;
        }

        public async Task AddtoQuestionSet(Guid questionsetId, Question question)
        {
            var targetsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = targetsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            var questiondata = _mapper.Map<DataModels.Question>(question);
            questionset.Questions.Add(questiondata);
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == targetsection.Id, targetsection));
        }

        public async Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId)
        {
            var parentsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset=parentsection.QuestionSets.FirstOrDefault(x=>x.Id== questionsetId);
            var questionmodels = _mapper.Map<IEnumerable<Question>>(questionset.Questions);
            return questionmodels;
        }

        public async Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId, Expression<Func<Question, bool>> predicate)
        {
            var parentsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = parentsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            var questionmodels = _mapper.Map<IQueryable<Question>>(questionset.Questions);
            return questionmodels.Where(predicate);
        }

        public async Task<bool> IsExistInQuestionSet(Guid questionsetId, Guid questionId)
        {
            var parentsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = parentsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            return questionset.Questions.Any(x => x.id == questionsetId);
        }

        public async Task MovetoQuestionSet(Guid questionId, Guid targetquestionsetId)
        {
            var originalsection = await GetParentSectionofQuestion(questionId);
            var originalqurstionset = originalsection.QuestionSets
                .FirstOrDefault(x => x.Questions
                .Any(x => x.id == questionId));
            var question = originalqurstionset.Questions
                .FirstOrDefault(x => x.id == questionId);
            var targetsection = originalsection;
            var targetquestionset = originalsection.QuestionSets
                .FirstOrDefault(x => x.Id == targetquestionsetId);
            if(targetquestionset == null)
            {
                targetsection = await GetParentSectionofQuestionSet(targetquestionsetId);
                targetquestionset = targetsection.QuestionSets
                    .FirstOrDefault(x => x.Id == targetquestionsetId);
            }
            originalqurstionset.Questions.Remove(question);
            targetquestionset.Questions.Add(question);
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == originalsection.Id, originalsection));
            if(targetsection == originalsection)
            {
                Context.AddCommand(() => _sections
                .ReplaceOneAsync(x => x.Id == targetsection.Id, targetsection));
            }
            

        }
    }
}
