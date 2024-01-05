using AutoMapper;
using BookManager.API.Data;
using BookManager.API.Models;
using System.Data.Common;
using System.Linq.Expressions;

namespace BookManager.API.ServiceProvider
{
    public class DefaultBookManager
        : IBookManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DefaultBookManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ChangeBookDescription(Guid book_id, string new_description)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if(data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            data_book.Description = new_description;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeBookImage(Guid book_id, Stream image_stream, string extension)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if(data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            IList<Task> tasks = new List<Task>();
            tasks.Add(this.DeleteImage(book_id));
            tasks.Add(this.SaveImage(book_id, image_stream, extension));
            await Task.WhenAll(tasks);
            await _context.SaveChangesAsync();
        }

        public async Task CreateBook(Book book)
        {
            var data_book = _mapper.Map<Data.Models.Book>(book);
            await _context.Books.AddAsync(data_book);
            var saved = await _context.SaveChangesAsync();
            if(!(saved > 0))
            {
                throw new Exception("Failed to save Book Data!");
            }
        }

        public async Task DeleteBook(Guid book_id)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if (data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            _context.Books.Remove(data_book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var datas = _context.Books.ToList();
            return _mapper.Map<IEnumerable<Book>>(datas);
        }

        public async Task<IEnumerable<Book>> GetAllBooks(Expression<Func<Book, bool>> predicate)
        {
            var data_predicate = _mapper.Map<Expression<Func<Data.Models.Book, bool>>>(predicate);
            var datas = _context.Books.Where(data_predicate);
            return _mapper.Map<IEnumerable<Book>>(datas);
        }

        public async Task<Book> GetBook(Guid book_id)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if (data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            return _mapper.Map<Book>(data_book);
        }

        public async Task<bool> IsBookExist(Guid book_id)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if(data_book == null)
            {
                return false;
            }
            return true;
        }

        public async Task SaveImage(Guid book_id, Stream image_stream, string extension)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if (data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            var saved_file_name = $"Image_{DateTime.UtcNow.Ticks}." + extension;
            var folder_name = "Assets";
            if (!Directory.Exists(folder_name))
            {
                Directory.CreateDirectory(folder_name);
            }
            var path = Path.Combine(folder_name,saved_file_name);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await image_stream.CopyToAsync(fs);
            }
            data_book.ImageUrl = path;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImage(Guid book_id)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if (data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            var image_path = data_book.ImageUrl;
            if (File.Exists(image_path))
            {
                File.Delete(image_path);
            }
            image_path = null;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasImage(Guid book_id)
        {
            var data_book = await _context.Books.FindAsync(book_id);
            if (data_book == null)
            {
                throw new Exception($"Book with Id = {book_id} is not exist!");
            }
            var image_path = data_book.ImageUrl;
            if (File.Exists(image_path))
            {
                return true;
            }
            return false;
        }
    }
}
