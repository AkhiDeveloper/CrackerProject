using BookManager.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        [HttpGet("Sets/{chapterId}")]
        public async Task<ActionResult<int>> GetTotalNumberOfSets(Guid chapterId)
        {
            try
            {
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSetQuestion/{chapterId}/{setNumber}")]
        public async Task<ActionResult<QuestionDTO>> GetSetQuestions(Guid chapterId, int setNumber = 1, 
            int offset = 0, int limit = 10, string orderBy = "", bool isAscending = true)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{questionId}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(Guid questionId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{chapterId}/{setNumber}")]
        public async Task<ActionResult> AddQuestion(Guid chapterId, int SetNumber, [FromForm]QuestionForm form)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("text/{questionId}")]
        public async Task<ActionResult<QuestionDTO>> ChangeText(Guid questionId, string new_text)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("image/{questionId}")]
        public async Task<ActionResult<QuestionDTO>> ChangeImage(Guid questionId, IFormFile imageFile)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("options/{questionId}")]
        public async Task<ActionResult<QuestionDTO>> ChangeOptions(Guid questionId, IEnumerable<OptionForm> options)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{questionId}")]
        public async Task<ActionResult> Delete(Guid questionId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
