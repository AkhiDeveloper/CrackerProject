using AutoMapper;
using BookManager.API.DTOs;
using BookManager.API.Models;
using BookManager.API.ServiceProvider;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BookManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionManager _questionManager;
        private readonly IChapterManager _chapterManager;
        private readonly IQuestionTextConverter _qsnTxtConverter;

        public QuestionsController(
            IMapper mapper, IQuestionManager questionManager, IChapterManager chapterManager, IQuestionTextConverter qsnTxtConverter)
        {
            _mapper = mapper;
            _questionManager = questionManager;
            _chapterManager = chapterManager;
            _qsnTxtConverter = qsnTxtConverter;
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

        [HttpGet("SetQuestion/{chapterId}/{setNumber}")]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetSetQuestions
            (Guid chapterId, int setNumber = 1, int offset = 0, int limit = 10, string orderBy = "", bool isAscending = true)
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
                var questions = await _questionManager.GetAllQuestionOfSet(chapterId, setNumber);
                IList<QuestionDTO> result = new List<QuestionDTO>();
                foreach( var question in questions)
                {
                    var questiondto = _mapper.Map<QuestionDTO>(question);
                    result.Add(questiondto);
                }
                return Ok(result);
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
                var question = await _questionManager.GetQuestion(questionId);
                var dto = _mapper.Map<QuestionDTO>(question);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{chapterId}/{setNumber}")]
        public async Task<ActionResult<QuestionDTO>> AddQuestion(Guid chapterId, int setNumber, [FromForm]QuestionForm form)
        {
            try
            {
                var question = _mapper.Map<Models.Question>(form);
                await _questionManager.CreateQuestion(chapterId, setNumber, question);
                if(form.ImageFile != null)
                {
                    var imageStream = new MemoryStream();
                    form.ImageFile.CopyTo(imageStream);
                    await _questionManager.ChangeImage(question.Id, imageStream);
                }
                var questionData = await _questionManager.GetQuestion(question.Id);
                _mapper.Map(questionData, question);
                var questionDto = _mapper.Map<QuestionDTO>(question);
                return Ok(questionDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{chapterId}/FromJsonFiles")]
        public async Task<IActionResult> AddQuestionsUsingJsonFile
            (Guid chapterId, [FromForm] UploadQuestionFromFileRequestBody requestBody)
        {
            return Ok();
        }

        [HttpPost("{chapterId}/FromRawFiles")]
        public async Task<IActionResult> AddQuestionsUsingRawTextFile
            (Guid chapterId, [FromForm] UploadQuestionFromFileRequestBody requestBody)
        {
            try
            {
                string[] allowableExtensions = { "txt", "csv" };
                if(allowableExtensions.Contains(Path.GetExtension(requestBody.QuestionRawText.FileName)))
                {
                    throw new Exception($"QuestionRawText File should be of file type \"{allowableExtensions}\"");
                }
                if (allowableExtensions.Contains(Path.GetExtension(requestBody.CorrectQuestionRawText.FileName)))
                {
                    throw new Exception($"QuestionRawText File should be of file type \"{allowableExtensions}\"");
                }
                var qsnString = String.Empty;
                var corrOptString = String.Empty;
                using(var stream = requestBody.QuestionRawText.OpenReadStream())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[stream.Length];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    qsnString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
                using (var stream = requestBody.CorrectQuestionRawText.OpenReadStream())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[stream.Length];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    corrOptString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
                var isParsed = _qsnTxtConverter.TryParse
                    (qsnString, corrOptString, out IEnumerable<Question> readedQuestions);
                if (!isParsed) { throw new Exception("Failed To Parse Question and Option Text"); }
                //var qsnSetNum = (await _questionManager.GetTotalNumberOfSet(chapterId)) + 1;
                //foreach(var question in readedQuestions)
                //{
                //    await _questionManager.CreateQuestion(chapterId, qsnSetNum, question);
                //}
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
                await _questionManager.ChangeText(questionId, new_text);
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
                Stream imageStream = new MemoryStream();
                await imageFile.CopyToAsync(imageStream);
                await _questionManager.ChangeImage(questionId, imageStream);
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
                var options_model = _mapper.Map<IList<Models.Option>>(options);
                await _questionManager.ChangeOptions(questionId, options_model);
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
                await _questionManager.DeleteQuestion(questionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
