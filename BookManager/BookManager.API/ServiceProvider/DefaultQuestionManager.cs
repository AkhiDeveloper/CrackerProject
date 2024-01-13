using AutoMapper;
using BookManager.API.Data;
using BookManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.API.ServiceProvider
{
    public class DefaultQuestionManager
        : IQuestionManager, IDisposable
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
            return chapter is null ? null : chapter.BookId;
        }

        public async Task ChangeImage(Guid questionId, Stream image)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question is null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var bookId = await this.GetBookIdOfQuestionId(questionId);
            string folderName = Path.Combine(bookId.ToString()??string.Empty,"Questions");
            string fileName = $"{questionId.ToString()}.png";
            await _fileStorage.UploadFile(fileName, image, folderName);
            question.ImageUri = Path.Combine(folderName, fileName);
            _context.SaveChanges();
        }

        public async Task ChangeOption(Guid questionId, int OptSN, Models.Option option)
        {
            var option_data = await GetOptionData(questionId, OptSN);
            _mapper.Map(option, option_data);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeOptionImage(Guid questionId, int opt_sn, Stream image)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question is null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var number_of_sets = await _context.Options.MaxAsync(x => x.SetNumber);
            if (number_of_sets > 1) throw new Exception("there are multiple option-set.");
            var option = await _context.Options.Where(x => x.QuestionId == question.Id).Where( x=> x.SN == opt_sn ).FirstOrDefaultAsync();
            if (option is null)
            {
                throw new Exception($"Option with sn = {opt_sn} is not found!");
            }
            var bookId = await this.GetBookIdOfQuestionId(questionId);
            string folderName = Path.Combine(bookId.ToString() ?? string.Empty, "Options");
            string fileName = $"{questionId.ToString()}_{opt_sn}.png";
            await _fileStorage.UploadFile(fileName, image, folderName);
            option.ImageUri = Path.Combine(folderName, fileName);
            _context.SaveChanges();
        }

        public async Task AddOptionsSet(Guid questionId, IEnumerable<Models.Option> options)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var qsn_opts = _context.Options.Where(x => x.QuestionId == question.Id);
            short setNumber = 1;
            if (qsn_opts.Any())
            {
                setNumber = (short)(qsn_opts.Max(x => x.SetNumber) + 1);
            }
            int i = 0;
            foreach(var option in options)
            {
                i++;
                option.SN = i;
                var option_data = _mapper.Map<Data.Models.Option>(option);
                option_data.QuestionId = questionId;
                option_data.SetNumber = setNumber;
                await _context.Options.AddAsync(option_data);
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
            if (question.Image is not null)
            {
                await ChangeImage(question.Id, question.Image);
            }
            foreach (var optsSet in question.OptionsSets)
            {
                await AddOptionsSet(question.Id, optsSet.Options);
            }
        }

        public async Task CreateQuestionAtLastSet(Guid chapterId, Models.Question question)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter == null)
            {
                throw new Exception($"Chapter with id = {chapterId} is not found!");
            }
            int set_sn = 1;
            var set = await _context.QuestionSets.Where(x => x.ChapterId == chapterId).OrderByDescending(x => x.SN).FirstOrDefaultAsync();
            if(set is not null)
            {
                set_sn = set.SN;
            }
            await CreateQuestion(chapterId, set_sn, question);
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
                foreach(var data_question in data_questions)
                {
                    questions.Add(await this.GetQuestion(data_question.Id));
                }
            }
            return questions;
        }

        public async Task<Stream> GetOptionImage(Guid questionId, int opt_sn)
        {
            var option = await this.GetOptionData(questionId, opt_sn);
            if(option == null)
            {
                throw new Exception("Option not found!");
            }
            if(option.ImageUri is null)
            {
                throw new Exception("Image is not found!");
            }
            var image = await _fileStorage.DownloadFile(option.ImageUri);
            return image;
        }

        public async Task<Models.Question> GetQuestion(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if(question == null)
            {
                throw new Exception("Question not found!");
            }
            //Stream? imageStream = question.ImageUri is null ? null : await _fileStorage.DownloadFile(question.ImageUri);
            return _mapper.Map<Models.Question>(question);
        }

        public async Task<Stream> GetQuestionImage(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception("Question not found!");
            }
            Stream? imageStream = question.ImageUri is null ? null : await _fileStorage.DownloadFile(question.ImageUri);
            if(imageStream == null)
            {
                throw new Exception("Image for this question is not found");
            }
            return imageStream;
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

        public void Dispose()
        {
            _fileStorage.Dispose();
        }

        public async Task ChangeOption(Guid questionId, short optionsSetSN, int OptSN, Option option)
        {
            var original_option = await this.GetOptionData(questionId, optionsSetSN, OptSN);
            _mapper.Map(option, original_option);
            //if (option.Image is not null)
            //{
            //    await ChangeOptionImage(questionId, OptSN, option.Image);
            //}
            await _context.SaveChangesAsync();
        }

        public async Task ChangeOptionImage(Guid questionId, short optionsSetSN, int opt_sn, Stream image)
        {
            var option = await this.GetOptionData(questionId, optionsSetSN, opt_sn);
            var bookId = await this.GetBookIdOfQuestionId(questionId);
            string folderName = Path.Combine(bookId.ToString() ?? string.Empty, "Options");
            string fileName = $"{questionId.ToString()}_{opt_sn}.png";
            await _fileStorage.UploadFile(fileName, image, folderName);
            option.ImageUri = Path.Combine(folderName, fileName);
            _context.SaveChanges();
        }

        public async Task AddOption(Guid questionId, Option option)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var optionSet = await _context.Options.Where(x => x.QuestionId == question.Id).MaxAsync(x => x.SetNumber);
            if(optionSet < 1) optionSet = 1;
            var option_data = _mapper.Map<Data.Models.Option>(option);
            option_data.SetNumber = optionSet;
            option_data.QuestionId = question.Id;
            await _context.Options.AddAsync(option_data);
            await _context.SaveChangesAsync();
        }

        public async Task AddOption(Guid questionId, short optionsSetSN, Option option)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var option_data = _mapper.Map<Data.Models.Option>(option);
            option_data.SetNumber = optionsSetSN;
            option_data.QuestionId = question.Id;
            await _context.Options.AddAsync(option_data);
            await _context.SaveChangesAsync();
        }

        private async Task<Data.Models.Option> GetOptionData(Guid questionId, int opt_sn)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question is null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var qsn_options = _context.Options.Where(x => x.QuestionId == question.Id);
            var number_of_sets = await qsn_options.MaxAsync(x => x.SetNumber);
            if (number_of_sets > 1) throw new Exception("there are multiple option-set.");
            var option = await qsn_options.Where(x => x.SN == opt_sn).FirstOrDefaultAsync();
            if (option is null)
            {
                throw new Exception($"Option with sn = {opt_sn} is not found!");
            }
            return option;
        }

        private async Task<Data.Models.Option> GetOptionData(Guid questionId, int opt_set_sn, int opt_sn)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question is null)
            {
                throw new Exception($"Question with id = {questionId} is not found!");
            }
            var qsn_options = _context.Options.Where(x => x.QuestionId == question.Id).Where(x => x.SetNumber == opt_set_sn);
            if (!await qsn_options.AnyAsync())
            {
                throw new Exception("Set doesn't exist!");
            }
            var option = await qsn_options.Where(x => x.SN == opt_sn).FirstOrDefaultAsync();
            if (option is null)
            {
                throw new Exception($"Option with sn = {opt_sn} is not found!");
            }
            return option;
        }
    }
}
