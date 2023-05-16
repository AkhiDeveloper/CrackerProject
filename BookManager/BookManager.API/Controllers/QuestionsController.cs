using AutoMapper;
using BookManager.API.DTOs;
using BookManager.API.ServiceProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionManager _questionManager;
        private readonly IChapterManager _chapterManager;

        public QuestionsController(
            IMapper mapper, IQuestionManager questionManager)
        {
            _mapper = mapper;
            _questionManager = questionManager;
        }

        [HttpGet("Sets/{chapterId}")]
        public async Task<ActionResult<int>> GetTotalNumberOfSets(Guid chapterId)
        {
            try
            {
                var chapter = await _chapterManager.GetChapterById(chapterId);
                if (chapter == null)
                {
                    throw new Exception($"Chapter with Id = {chapterId} is not found!");
                }
                var sets = await _questionManager.GetTotalNumberOfSet(chapterId);
                return Ok(sets);
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
                var chapter = await _chapterManager.GetChapterById(chapterId);
                if (chapter == null)
                {
                    throw new Exception($"Chapter with Id = {chapterId} is not found!");
                }
                var sets = await _questionManager.GetTotalNumberOfSet(chapterId);
                if(setNumber > sets)
                {
                    throw new Exception($"Set with SN = {setNumber} is not found!");
                }
                var questions = _questionManager.GetAllQuestionOfSet(chapterId, setNumber);
                var questionsdto = _mapper.Map<QuestionDTO>(questions);
                return Ok(questionsdto);
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
