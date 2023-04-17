using AutoMapper;
using BookManager.API.DTOs;
using BookManager.API.ServiceProvider;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        private readonly IMapper _mapper;

        public BooksController(IBookManager bookManager, IMapper mapper)
        {

            _bookManager = bookManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll
            (int offset = 0, int limit = 10, string orderBy = "", bool isAscending = true)
        {
            try
            {
                var all_models = await _bookManager.GetAllBooks();
                var filtered_models = all_models.Skip(offset).Take(limit);
                var filtered_dto = _mapper.Map<IEnumerable<BookDTO>>(filtered_models);
                return Ok(new { records = filtered_dto, total = all_models.Count() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetById(Guid id)
        {
            try
            {
                var model = await _bookManager.GetBook(id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] BookForm bookForm)
        {
            try
            {
                var model = _mapper.Map<Models.Book>(bookForm);
                await _bookManager.CreateBook(model);
                if(bookForm.Image != null)
                {
                    var extension = bookForm.Image.FileName.Split('.')[1];
                    await _bookManager.SaveImage(model.Id, bookForm.Image.OpenReadStream(), extension);
                }
                return Ok($"Book is created with Id = {model.Id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromForm] BookEditForm bookForm)
        {
            try
            {
                var isAny = await _bookManager.IsBookExist(id);
                if (!isAny)
                {
                    return NoContent();
                }
                IList<Task> Tasks = new List<Task>();
                if (!String.IsNullOrWhiteSpace(bookForm.Description))
                {
                    Tasks.Add(_bookManager.ChangeBookDescription(id, bookForm.Description));
                }
                if(bookForm.Image != null)
                {
                    var extension = bookForm.Image.FileName.Split('.')[1];  
                    Tasks.Add(_bookManager.ChangeBookImage(id, bookForm.Image.OpenReadStream(), extension));
                }
                await Task.WhenAll(Tasks);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/Description")]
        public async Task<ActionResult> ChangeDescription(Guid id, string newdescription)
        {
            try
            {
                var isAny = await _bookManager.IsBookExist(id);
                if (!isAny)
                {
                    return NoContent();
                }
                await _bookManager.ChangeBookDescription(id, newdescription);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/Image")]
        public async Task<ActionResult> ChangeImage(Guid id, IFormFile newImage)
        {
            try
            {
                var isAny = await _bookManager.IsBookExist(id);
                if (!isAny)
                {
                    return NoContent();
                }
                if(newImage != null)
                {
                    var extension = newImage.FileName.Split(".")[1];    
                    await _bookManager.ChangeBookImage(id, newImage.OpenReadStream(), extension);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var isAny = await _bookManager.IsBookExist(id);
                if (!isAny)
                {
                    return NoContent();
                }
                await _bookManager.DeleteBook(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
