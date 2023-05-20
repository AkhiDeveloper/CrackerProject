using AutoMapper;
using Data = BookManager.API.Data;
using BookManager.API.Models;
using System.Threading.Tasks.Dataflow;

namespace BookManager.API.ServiceProvider
{
    public class DefaultChapterManager
        : IChapterManager
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DefaultChapterManager(
            Data.ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ChangeChapterDescription
            (Guid chapterId, string newdescription)
        {
            var chapter_data = await _context.Chapters.FindAsync(chapterId);
            if (chapter_data != null)
            {
                chapter_data.Description = newdescription;
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateChapter
            (Guid bookId, Chapter chapter, Guid? chapterId = null)
        {
            if(_context.Books.FindAsync(bookId).Result == null)
            {
                throw new Exception($"Book with Id = {bookId} is not exist!");
            }
            if (_context.Chapters.FindAsync(chapterId).Result == null)
            {
                throw new Exception($"Parent Chapter with Id = {chapterId} is not exist!");
            }
            var chapter_data = _mapper.Map<Data.Models.Chapter>(chapter);
            var chapter_count = _context.Chapters.Where(x => x.BookId == bookId && x.ParentChapterId == chapterId).Count();
            chapter_data.BookId = bookId;
            chapter_data.ParentChapterId = chapterId;
            chapter_data.SN = chapter_count + 1;
            await _context.Chapters.AddAsync(chapter_data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChapter(Guid chapterId)
        {
            var chapter_data = await _context.Chapters.FindAsync(chapterId);
            if(chapter_data == null)
            {
                throw new Exception($"Chapter with Id = {chapterId} is not found!");
            }
            _context.Chapters.Remove(chapter_data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Chapter>> GetAllChapter(Guid bookId, Guid? chapterId = null)
        {
            if (_context.Books.FindAsync(bookId).Result == null)
            {
                throw new Exception($"Book with Id = {bookId} is not exist!");
            }
            if (chapterId != null && _context.Chapters.FindAsync(chapterId).Result == null)
            {
                throw new Exception($"Chapter with Id = {chapterId} is not exist!");
            }
            var chapters_data = _context.Chapters.Where(x => x.BookId == bookId).Where(x => x.ParentChapterId == chapterId);
            var chapters_model = _mapper.Map<IEnumerable<Chapter>>(chapters_data);
            return chapters_model;
        }

        public async Task<Chapter> GetChapterById(Guid chapterId)
        {
            var chapter_data = await _context.Chapters.FindAsync(chapterId);
            var chapter = _mapper.Map<Chapter>(chapter_data);
            return chapter;
        }

        public async Task MoveChapter(Guid targetBookId, Guid chapterId, Guid? targetChapterId = null)
        {
            if (_context.Books.FindAsync(chapterId).Result != null)
            {
                throw new Exception($"Chapter with Id = {chapterId} is already exist!");
            }
            if (_context.Books.FindAsync(targetBookId).Result != null)
            {
                throw new Exception($"Target Book with Id = {targetBookId} is already exist!");
            }
            if (_context.Chapters.FindAsync(targetBookId).Result != null)
            {
                throw new Exception($"Target Chapter with Id = {targetBookId} is already exist!");
            }
            var chapter_data = await _context.Chapters.FindAsync(chapterId);
            chapter_data.BookId = targetBookId;
            chapter_data.ParentChapterId = chapterId;
            await _context.SaveChangesAsync();
        }
    }
}
