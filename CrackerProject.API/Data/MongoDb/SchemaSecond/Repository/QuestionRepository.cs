using CrackerProject.API.Interfaces;
using AutoMapper;
using System.Linq.Expressions;
using MongoDB.Driver;
using Humanizer;
using DataModel = CrackerProject.API.Data.MongoDb.SchemaSecond.Model;
using CrackerProject.API.Model.Book;
using ServiceStack;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Repository
{
    public class QuestionRepository : BaseRepository<Question, DataModel.Question, Guid>, IQuestionRepository
    {
        private readonly IMongoCollection<DataModel.Section> _sections;
        private readonly IMongoCollection<DataModel.Chapter> _chapters;

        public QuestionRepository(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
            _sections = Context.GetCollection<DataModel.Section>
                (nameof(DataModel.Section).Pluralize());
            _chapters = Context.GetCollection<DataModel.Chapter>
                (nameof(DataModel.Chapter).Pluralize());
        }

        private async Task<DataModel.Section> GetParentSectionofQuestion(Guid question_id)
        {
            var parentsectioncursor = await _sections.FindAsync(s =>
            s.QuestionSets.Any(qs =>
            qs.QuestionIds.Any(q =>
            q == question_id)));
            var parentsection = parentsectioncursor.FirstOrDefault();
            if (parentsection == null)
            {
                throw new NullReferenceException
                    ($"Question with id ={question_id} is not found.");
            }
            return parentsection;
        }

        private async Task<DataModel.Section> GetParentSectionofQuestionSet(Guid questionset_id)
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
            base.AddAsync(obj);
        }

        public override async Task<IEnumerable<Question>> Find(Expression<Func<Question, bool>> expression)
        {
            return await base.Find(expression);
        }

        public override Task<IList<Question>> Find<U>(string fieldname, U fieldvalue)
        {
            return base.Find(fieldname, fieldvalue);
        }

        public override async Task<IEnumerable<Question>> GetAll()
        {
            return await base.GetAll();
        }

        public override async Task<Question> GetById(Guid id)
        {
            return await base.GetById(id);
        }

        public override void RemoveAsync(Guid id)
        {
            base.RemoveAsync(id);
        }

        public override void UpdateAsync(Question obj)
        {
            base.UpdateAsync(obj);
        }

        public async Task AddtoQuestionSet(Guid questionsetId, Question question)
        {
            var targetsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = targetsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            var questiondata = _mapper.Map<Model.Question>(question);
            questionset.QuestionIds.Add(questiondata.Id);
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == targetsection.Id, targetsection));
            AddAsync(question);
        }

        public async Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId)
        {
            var parentsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = parentsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            var questions = new List<Question>();
            foreach(var questionid in questionset.QuestionIds)
            {
                questions.Add(await GetById(questionid));
            }
            return questions;
        }

        public async Task<IEnumerable<Question>> GetfromQuestionSet(Guid questionsetId, Expression<Func<Question, bool>> predicate)
        {
            var parentsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = parentsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            IList<Question> questions = new List<Question>();
            foreach (var questionid in questionset.QuestionIds)
            {
                questions.Add(await GetById(questionid));
            }
            return questions.AsQueryable().Where(predicate);
        }

        public async Task<bool> IsExistInQuestionSet(Guid questionsetId, Guid questionId)
        {
            var parentsection = await GetParentSectionofQuestionSet(questionsetId);
            var questionset = parentsection.QuestionSets.FirstOrDefault(x => x.Id == questionsetId);
            return questionset.QuestionIds.Any(x => x == questionId);
        }

        public async Task MovetoQuestionSet(Guid questionId, Guid targetquestionsetId)
        {
            if (!await IsExist(questionId))
                throw new NullReferenceException($"Question with Id = {questionId} is not found.");
            var targetsection = await GetParentSectionofQuestionSet(targetquestionsetId);
            var targetquestionset = targetsection.QuestionSets.FirstOrDefault(x => x.Id == targetquestionsetId);
            var originalsection = await GetParentSectionofQuestion(questionId);
            var originalquestionset = originalsection.QuestionSets
                .FirstOrDefault(x => x.QuestionIds
                .Any(x => x == questionId));
            originalquestionset.QuestionIds.Remove(questionId);
            targetquestionset.QuestionIds.Add(questionId);
            if(targetsection != originalsection)
            {
                Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == originalsection.Id, originalsection));
            }
            Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == targetsection.Id, targetsection));
        }

        public async Task AssignSectionQuestionstoChapter(Guid chapter_id, Guid section_id)
        {
            var chaptercursor = await _chapters.FindAsync(x => x.Id == chapter_id);
            var chapter = await chaptercursor.FirstOrDefaultAsync();
            if (chapter == null)
                throw new NullReferenceException($"Chapter with Id = {chapter_id} is not found.");
            if(!await _sections.Find(x=>x.Id==section_id).AnyAsync())
                throw new NullReferenceException($"Section with Id = {section_id} is not found.");
            chapter.AssociatedSectionIds.Add(section_id);
            Context.AddCommand(() => _chapters.ReplaceOneAsync(x => x.Id == chapter_id, chapter));
        }

        public async Task<IEnumerable<Question>> GetChapterQuestions(Guid chapter_id)
        {
            var chaptercursor = await _chapters.FindAsync(x => x.Id == chapter_id);
            var chapter = await chaptercursor.FirstOrDefaultAsync();
            if (chapter == null)
                throw new NullReferenceException($"Chapter with Id = {chapter_id} is not found.");
            List<Question> answer = new List<Question>();
            foreach(var sectionid in chapter.AssociatedSectionIds)
            {
                var section = await _sections.FindAsync(x => x.Id == sectionid).Result.FirstOrDefaultAsync();
                foreach(var questionset in section.QuestionSets)
                {
                    foreach(var questionid in questionset.QuestionIds)
                    {
                        answer.Add(await GetById(questionid));
                    }
                }
            }
            return answer;
        }

        public async Task<IEnumerable<Question>> GetChapterQuestions(Guid chapter_id, Expression<Func<Question, bool>> predicate)
        {
            var chaptercursor = await _chapters.FindAsync(x => x.Id == chapter_id);
            var chapter = await chaptercursor.FirstOrDefaultAsync();
            if (chapter == null)
                throw new NullReferenceException($"Chapter with Id = {chapter_id} is not found.");
            List<Question> answer = new List<Question>();
            foreach (var sectionid in chapter.AssociatedSectionIds)
            {
                var section = await _sections.FindAsync(x => x.Id == sectionid).Result.FirstOrDefaultAsync();
                foreach (var questionset in section.QuestionSets)
                {
                    foreach (var questionid in questionset.QuestionIds)
                    {
                        answer.Add(await GetById(questionid));
                    }
                }
            }
            return answer.AsQueryable().Where(predicate);
        }

        public async Task UnAssignSectionQuestionfromChapter(Guid chapter_id, Guid section_id)
        {
            var chaptercursor = await _chapters.FindAsync(x => x.Id == chapter_id);
            var chapter = await chaptercursor.FirstOrDefaultAsync();
            if (chapter == null)
                throw new NullReferenceException($"Chapter with Id = {chapter_id} is not found.");
            chapter.AssociatedSectionIds.Remove(section_id);
            Context.AddCommand(() => _chapters.ReplaceOneAsync(x => x.Id == chapter_id, chapter));
        }

        public async Task<IEnumerable<Guid>> GetChapterQuestionIds(Guid chapter_id)
        {
            var chaptercursor = await _chapters.FindAsync(x => x.Id == chapter_id);
            var chapter = await chaptercursor.FirstOrDefaultAsync();
            if (chapter == null)
                throw new NullReferenceException($"Chapter with Id = {chapter_id} is not found.");
            List<Guid> answer = new List<Guid>();
            foreach (var sectionid in chapter.AssociatedSectionIds)
            {
                var section = await _sections.FindAsync(x => x.Id == sectionid).Result.FirstOrDefaultAsync();
                foreach(var questionset in section.QuestionSets)
                {
                    answer.AddRange(questionset.QuestionIds);
                }
            }
            return answer;
        }
    }
}
