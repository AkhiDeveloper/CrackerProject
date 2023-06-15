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
        private readonly IFileStorage _fileStorage;

        public DefaultQuestionManager(IMapper mapper,
            ApplicationDbContext context,
            IFileStorage fileStorage)
        {
            _mapper = mapper;
            _context = context;
            _fileStorage = fileStorage;
        }

        private async Task<Guid?> GetBookIdOfQuestionId(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question is null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var questionSet = await _context.QuestionSets.FindAsync(question.ParentSetId);
            if (questionSet is null)
            {
                throw new Exception($"QuestionSet is not found!");
            }
            var chapter = await _context.Chapters.FindAsync(questionSet.ChapterId);
            while (chapter is not null)
            {
                if(chapter.BookId != Guid.Empty)
                {
                    break;
                }
                chapter = await _context.Chapters.FindAsync(chapter.ParentChapterId);
            }
            return chapter is null?null:chapter.BookId;
        }

        public async Task ChangeImage(Guid questionId, Stream image)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question is null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var bookId = await this.GetBookIdOfQuestionId(questionId);
            string folderName = bookId.ToString()??string.Empty;
            string fileName = $"{questionId.ToString()}.png";
            await _fileStorage.UploadFile(fileName, image, folderName);
            question.ImageUri = Path.Combine(folderName, fileName);
            _context.SaveChanges();
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

        public Task ChangeOptionImage(Guid questionId, int opt_sn, Stream image)
        {
            throw new NotImplementedException();
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

        public async Task CreateQuestion(Guid chapterId, int set_no, Models.Question question)
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

        public async Task<IEnumerable<Models.Question>> GetAllQuestionOfLastSet(Guid chapterId)
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

        public async Task<IEnumerable<Models.Question>> GetAllQuestions(Guid chapterId)
        {
            IEnumerable<Data.Models.QuestionSet> questionSets = _context.QuestionSets.Where(x => x.ChapterId == chapterId);
            IList<Models.Question> questions = new List<Models.Question>();
            foreach (var questionSet in questionSets)
            {
                var data_questions = _context.Questions.Where(x => x.ParentSetId == questionSet.Id);
                var model_questions = _mapper.Map<IList<Models.Question>>(data_questions);
                foreach(var model in model_questions)
                {
                    questions.Add(model);
                }
            }
            return questions;
        }

        public Task<string> GetOptionImageUri(Guid questionId, int opt_sn)
        {
            throw new NotImplementedException();
        }

        public async Task<Models.Question> GetQuestion(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            return _mapper.Map<Models.Question>(question);
        }

        public Task<string> GetQuestionImageUri(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalNumberOfSet(Guid chapterId)
        {
            var questionSets =  _context.QuestionSets.Where(x => x.ChapterId == chapterId);
            return await questionSets.CountAsync();
        }

        private async Task<Data.Models.QuestionSet> CreateQuestionSet(Guid chapterId,int Sn, string Description)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if(chapter == null)
            {
                throw new Exception("Chapter is not found!");
            }
            Data.Models.QuestionSet questionSet = new Data.Models.QuestionSet();
            questionSet.Id = Guid.NewGuid();
            if(await _context.QuestionSets.Where(x => x.ChapterId == chapterId).AnyAsync(x => x.SN == Sn))
            {
                throw new Exception($"Set with Sn = {Sn} is already exist!");
            }
            questionSet.Description = Description;
            questionSet.SN = Sn;
            questionSet.ChapterId = chapterId;
            await _context.QuestionSets.AddAsync(questionSet);
            await _context.SaveChangesAsync();
            return questionSet;
        }
    }
}
