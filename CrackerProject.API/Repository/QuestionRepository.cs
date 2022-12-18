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
            q.Id == question_id)));
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

        public override void AddAsync(Question obj)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Question>> Find(Expression<Func<Question, bool>> expression)
        {
            var questions = new List<Question>();
            var dataexpression = _mapper.Map<Expression<Func<DataModels.Question, bool>>>(expression);
            var sectioncursors = await _sections
                .FindAsync(s=>s.QuestionSets
                .Any(qs => qs.Questions
                .Any(dataexpression.Compile())));
            var sections = sectioncursors.ToList();
            foreach (var section in sections)
            {
                foreach (var questionset in section.QuestionSets)
                {
                    questions.AddRange(_mapper
                        .Map<IEnumerable<Question>>(questionset.Questions));
                }
            }
            return questions;
        }

        public override Task<IList<Question>> Find<U>(string fieldname, U fieldvalue)
        {
            return base.Find(fieldname, fieldvalue);
        }

        public override async Task<IEnumerable<Question>> GetAll()
        {
            var questions = new List<Question>();
            var sectioncursors = await _sections.FindAsync(Builders<DataModels.Section>.Filter.Empty);
            var sections = sectioncursors.ToList();
            foreach(var section in sections)
            {
                foreach(var questionset in section.QuestionSets)
                {
                    questions.AddRange(_mapper
                        .Map<IEnumerable<Question>>(questionset.Questions));
                }
            }
            return questions;
        }

        public override async Task<Question> GetById(Guid id)
        {
            var parentsection = await GetParentSectionofQuestion(id);
            var questionset = parentsection.QuestionSets
                .FirstOrDefault(x => x.Questions.Any(x => x.Id == id));
            var questiondata = questionset.Questions.FirstOrDefault(x => x.Id == id);
            var questionmodel = _mapper.Map<Question>(questiondata);
            return questionmodel;
        }

        public override async void RemoveAsync(Guid id)
        {
            var parentsection = await GetParentSectionofQuestion(id);
            var questionset = parentsection.QuestionSets
                .FirstOrDefault(x => x.Questions.Any(x => x.Id == id));
            var question = questionset.Questions.FirstOrDefault(x => x.Id == id);
            questionset.Questions.Remove(question);
            Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == parentsection.Id, parentsection));
        }

        public override async void UpdateAsync(Question obj)
        {
            var parentsection = await GetParentSectionofQuestion(obj.Id);
            var questionset = parentsection.QuestionSets
                .FirstOrDefault(x => x.Questions.Any(x => x.Id == obj.Id));
            var question = questionset.Questions.FirstOrDefault(x => x.Id == obj.Id);
            _mapper.Map(obj, question);
            Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == parentsection.Id, parentsection));
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
            return questionset.Questions.Any(x => x.Id == questionsetId);
        }

        public async Task MovetoQuestionSet(Guid questionId, Guid targetquestionsetId)
        {
            var originalsection = await GetParentSectionofQuestion(questionId);
            var originalqurstionset = originalsection.QuestionSets
                .FirstOrDefault(x => x.Questions
                .Any(x => x.Id == questionId));
            var question = originalqurstionset.Questions
                .FirstOrDefault(x => x.Id == questionId);
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
