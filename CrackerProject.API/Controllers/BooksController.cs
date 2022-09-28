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
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(
            IBookRepository? bookRepository, 
            IUnitOfWork? unitofwork)
        {
            _bookRepository = bookRepository;
            _unitOfWork=unitofwork;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromBody] BookVM value)
        {
            var newbook = new Book()
            {
                Description = value.Description,
                CreatedDateTime = DateTime.Now,
            };

            _bookRepository.Add(newbook);

            await _unitOfWork.Commit();

            var book = await _bookRepository.GetById(newbook.Id);

            if(book == null)
            {
                return NotFound($"{newbook} is failed to save book");
            }

            return Ok(book);
        }
    }
}
