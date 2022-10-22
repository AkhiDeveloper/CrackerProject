using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;
using CrackerProject.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrackerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookSectionRepository _booksectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BooksController(
            IBookRepository? bookRepository,
            IUnitOfWork? unitofwork,
            IBookSectionRepository booksectionRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitofwork;
            _booksectionRepository = booksectionRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<BookResponse>> Post([FromBody] BookCreationForm value)
        {
            var newbook = _mapper.Map<Book>(value);

            _bookRepository.Add(newbook);

            await _unitOfWork.Commit();

            var book = await _bookRepository.GetById(newbook.Id);

            if (book == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"{newbook} is failed to save book"
                }
                );
            }

            var responsemodel = _mapper.Map<BookResponse>(book);

            return Ok(new
            {
                success = true,
                response = responsemodel,
            });
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<BookResponse>>> GetAll()
        {
            var books = await _bookRepository.GetAll();

            if (books == null)
            {
                return NotFound(new
                {
                    success = false,
                    Message = "Failed to get books or books are not available"
                });
            }

            var responsemodel = _mapper.Map<IEnumerable<BookResponse>>(books);

            return Ok(new
            {
                success = true,
                response = responsemodel,
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> Get(Guid id)
        {
            var book = await _bookRepository.GetById(id);

            if (book == null)
            {
                return NotFound(new
                {
                    success = false,
                    Message = $"Book of id = {id} is not available"
                });
            }

            var responsemodel = _mapper.Map<BookResponse>(book);
            return Ok(new
            {
                success = true,
                response = responsemodel,
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookResponse>> Update(Guid id, BookCreationForm form)
        {
            var book = await _bookRepository.GetById(id);

            if(book == null)
            {
                return NotFound($"Book of {id} not found");
            }

            _mapper.Map(form, book);

            _bookRepository.Update(book);

            var issaved=await _unitOfWork.Commit();

            if (issaved == false)
            {
                return NotFound($"Failed to update Book of id = {id}");
            }

            var checkbook = await _bookRepository.GetById(id);

            var responsemodel = _mapper.Map<BookResponse>(checkbook);

            return Ok(new
            {
                success = true,
                response = responsemodel,
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BookResponse>> Delete(Guid id)
        {
            var storedbook = await _bookRepository.GetById(id);

            if(storedbook == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Book with id = {id} is not existed",
                });
            }

            _bookRepository.Remove(id);
            var iscompleted = await _unitOfWork.Commit();

            if(iscompleted == false)
            {
                return NotFound($"{storedbook} is failed to delete");
            }

            var responsemodel = _mapper.Map<BookResponse>(storedbook);

            return Ok(new
            {
                success = true,
                message = $"Book with id = {id} is deleted",
                response = responsemodel,
            });

        }
        
        [HttpPost("{id}/AddBookSection")]
        public async Task<ActionResult<BookSectionResponse>> AddBookSection(Guid id, [FromBody] BookSectionCreationForm sectionform)
        {
            var book = await _bookRepository.GetById(id);

            if(book == null)
            {
                return NotFound(new 
                { 
                    Success =  false,
                    Message = $"Book with {id} is not found.", 
                });
            }

            if(sectionform == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Invalid Data in section form.",
                    Model = sectionform,
                });
            }

            var section = _mapper.Map<BookSection>(sectionform);

            section.BookId = id;

            _booksectionRepository.Add(section);

            var result = await _unitOfWork.Commit();

            if(result == false)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Failed to Save Book Section Data.",
                    Model = section,
                });
            }
            var booksection = await _booksectionRepository.GetById(section.Id);

            if(booksection == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Failed to Save Book Section Data.",
                    Model = section,
                });
            }

            var responsemodel = _mapper.Map<BookSectionResponse>(booksection);

            return Ok(new { 
                Success = true,
                Message="Sucessfully saved Book section.",
                Model =  responsemodel,
            });
        }

        [HttpGet("{id}/GetBookSection")]
        public async Task<ActionResult<IEnumerable<BookSectionResponse>>> GetBookSection(Guid id)
        {
            try
            {
                var book = await _bookRepository.GetById(id);

                if (book == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Book with {id} is not found.",
                    });
                }

                var filteredsection = await _booksectionRepository.Find("book_id", id);

                if(filteredsection == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Book section not found of book with {id}"
                    });
                }

                var responsemodel = _mapper.Map<IList<BookSectionResponse>>(filteredsection);

                return Ok(new
                {
                    success = true,
                    response = responsemodel,
                });

            }
            catch(Exception e)
            {
                return BadRequest(new
                {
                    success = false,
                    message = e.Message,
                });
            }


        }
    }
}
