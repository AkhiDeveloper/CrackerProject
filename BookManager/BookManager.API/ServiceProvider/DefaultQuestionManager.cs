using AutoMapper;
using BookManager.API.Data;
using BookManager.API.Data.Models;
using BookManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.API.ServiceProvider
{
    public class DefaultQuestionManager
        : IQuestionManager
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public DefaultQuestionManager(IMapper mapper,
            ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task ChangeImage(Guid questionId, byte[] image)
        {
            throw new NotImplementedException();
        }

        public async Task ChangeOption(Guid questionId, int OptSN, Models.Option option)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if(question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var orginal_option = question.Options.Where(x => x.SN == OptSN).FirstOrDefault();
            _mapper.Map(option, orginal_option);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeOptions(Guid questionId, IEnumerable<Models.Option> options)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            question.Options.Clear();
            await _context.SaveChangesAsync();
            int i = 0;
            foreach(var option in options)
            {
                i++;
                option.SN = i;
                var option_data = _mapper.Map<Data.Models.Option>(option);  
                question.Options.Add(option_data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ChangeSN(Guid questionId, int SN)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            question.SN = SN;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeText(Guid questionId, string text)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            question.Text = text;
            await _context.SaveChangesAsync();
        }

        public async Task CreateQuestion(Guid chapterId, int set_no, Question question)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if(chapter == null)
            {
                throw new Exception($"Chapter with id = {chapterId} is not found!");
            }
            var set = await _context.QuestionSets.Where(x => x.ChapterId == chapterId).FirstOrDefaultAsync(x => x.SN == set_no);
            if (set == null)
            {
                set = await this.CreateQuestionSet(chapterId, set_no, "");
            }
            var data_question = _mapper.Map<Data.Models.Question>(question);
            data_question.ParentSetId = set.Id;
            await _context.Questions.AddAsync(data_question);
            await _context.SaveChangesAsync();
        }

        public async Task CreateQuestionAtLastSet(Guid chapterId, Models.Question question)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter == null)
            {
                throw new Exception($"Chapter with id = {chapterId} is not found!");
            }
            var set = await _context.QuestionSets.Where(x => x.ChapterId == chapterId).OrderByDescending(x => x.SN).FirstOrDefaultAsync();
            if(set == null)
            {
                set = await this.CreateQuestionSet(chapterId, 1, "");
            }
            var data_question = _mapper.Map<Data.Models.Question>(question);
            data_question.ParentSetId = set.Id;
            await _context.Questions.AddAsync(data_question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestion(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with Id = {questionId} is not found!");
            }
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionSet(Guid chapterId, int SN)
        {
            var questionSet = await _context.QuestionSets.Where(x => x.ChapterId == chapterId).FirstOrDefaultAsync(x => x.SN == SN);
            if(questionSet == null)
            {
                throw new Exception($"Question Set in chapterId = {chapterId} of SN = {SN} is not found!");
            }
            var anyQuestion = await _context.Questions.Where(x => x.ParentSetId == questionSet.Id).AnyAsync();
            if (anyQuestion)
            {
                throw new Exception("Question are in this Set. Delete All Question of this Set and re-try");
            }
            _context.QuestionSets.Remove(questionSet);
           await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.Question>> GetAllQuestionOfLastSetAsync(Guid chapterId)
        {
            var questionSet = await _context.QuestionSets.Where(x => x.ChapterId == chapterId).OrderByDescending(x => x.SN).FirstOrDefaultAsync();
            if (questionSet == null)
            {
                throw new Exception($"No Question Set in chapterId = {chapterId} is not found!");
            }
            var data_questions = _context.Questions.Where(x => x.ParentSetId == questionSet.Id);
            var models = _mapper.Map<IEnumerable<Models.Question>>(data_questions);
            return models;
        }

        public async Task<IEnumerable<Models.Question>> GetAllQuestionOfSet(Guid chapterId, int SetSN)
        {
            var questionSet = await _context.QuestionSets.Where(x => x.ChapterId == chapterId).FirstOrDefaultAsync(x => x.SN == SetSN);
            if (questionSet == null)
            {
                throw new Exception($"Question Set in chapterId = {chapterId} of SN = {SetSN} is not found!");
            }
            var data_questions = _context.Questions.Where(x => x.ParentSetId == questionSet.Id);
            var models = _mapper.Map<IEnumerable<Models.Question>>(data_questions);
            return models;
        }

        

        public int GetTotalNumberOfSet(Guid chapterId)
        {
            throw new NotImplementedException();
        }

        private async Task<Data.Models.QuestionSet> CreateQuestionSet(Guid chapterId,int Sn, string Description)
        {
            Data.Models.QuestionSet questionSet = new Data.Models.QuestionSet();
            questionSet.Id = Guid.NewGuid();
            if(await _context.QuestionSets.Where(x => x).AnyAsync(x => x.SN == Sn))
            {
                throw new Exception($"Set with Sn = {Sn} is already exist!");
            }
            questionSet.Description = Description;
            questionSet.SN = Sn;
            return questionSet;
        }
    }
}
