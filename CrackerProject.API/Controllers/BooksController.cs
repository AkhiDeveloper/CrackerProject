using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISectionRepository _sectionRepository;

        public BooksController(
            IBookRepository bookRepository,
            IUnitOfWork unitofwork,
            ISectionRepository sectionRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _sectionRepository = sectionRepository;
        }

        [HttpPost]
        public async Task<ActionResult<BookResponse>> Post([FromBody] BookCreationForm value)
        {
            try
            {
                var newbook = _mapper.Map<Book>(value);
                _bookRepository.Add(newbook);
                await _unitOfWork.Commit();
                var responsemodel = _mapper.Map<BookResponse>(newbook);
                return Ok(responsemodel);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll()
        {
            try
            {
                var books = await _bookRepository.GetAll();

                var responsemodel = _mapper.Map<IEnumerable<BookResponse>>(books);

                return Ok(responsemodel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> Get(Guid id)
        {
            try
            {
                var book = await _bookRepository.GetById(id);
                var responsemodel = _mapper.Map<BookResponse>(book);
                return Ok(responsemodel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookResponse>> Update(Guid id, BookCreationForm form)
        {
            try
            {
                var book = await _bookRepository.GetById(id);
                _mapper.Map(form, book);
                _bookRepository.Update(book);
                var issaved = await _unitOfWork.Commit();
                if (issaved == false)
                {
                    return BadRequest($"Failed to update Book of id = {id}");
                }
                var responsemodel = _mapper.Map<BookResponse>(book);
                return Ok(responsemodel);
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BookResponse>> Delete(Guid id)
        {
            try
            {
                _bookRepository.RemoveAsync(id);
                await _sectionRepository.RemoveBookSections(id, DeleteType.AssociatedAlso);
                var iscompleted = await _unitOfWork.Commit();
                if (iscompleted == false)
                {
                    return BadRequest($"{id} is failed to delete");
                }

                return Ok();
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }
        
        [HttpPost("{id}/Section")]
        public async Task<ActionResult<SectionResponse>> AddBookSection(Guid id, [FromBody] SectionCreationForm sectionform)
        {
            try
            {
                var section = _mapper.Map<Section>(sectionform);
                await _sectionRepository.AddtoBook(id, section);

                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest("Failed to Save Book Section Data.");
                }

                var responsemodel = _mapper.Map<SectionResponse>(section);

                return Ok(responsemodel);
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
            
        }

        [HttpGet("{id}/Section")]
        public async Task<ActionResult<IEnumerable<SectionResponse>>> GetBookSection(Guid id)
        {
            try
            {
                var sections = await _sectionRepository.GetfromBook(id);

                var responsemodel = _mapper.Map<IList<SectionResponse>>(sections);

                return Ok(responsemodel);

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
    }
}
