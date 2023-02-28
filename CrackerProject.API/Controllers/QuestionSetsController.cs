using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model.Book;
using CrackerProject.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrackerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionSetsController : ControllerBase
    {
        private readonly IQuestionSetRepository _questionSetRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionSetsController(
            IQuestionSetRepository questionSetRepository, 
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _questionSetRepository = questionSetRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
            _unitOfWork=unitOfWork;
        }

        [HttpGet("{id}/Questions")]
        public async Task<ActionResult<IEnumerable<ViewModel.QuestionResponse>>> GetQuestionSetQuestions(Guid id)
        {
            try
            {
                var questions = await _questionRepository.GetfromQuestionSet(id);
                var questionsview = _mapper.Map<IEnumerable<ViewModel.QuestionResponse>>(questions);
                return Ok(questionsview);
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost("{id}/Questions")]
        public async Task<ActionResult> PostQuestionSetQuestion(Guid id, [FromForm] QuestionForm questionForm)
        {
            try
            {
                var questionmodel = _mapper.Map<Question>(questionForm);
                await _questionRepository.AddtoQuestionSet(id, questionmodel);
                var issaved = await _unitOfWork.Commit();
                if(issaved == false)
                {
                    return BadRequest($"Fail to save Question to QuestionSet of Id = {id}");
                }
                return Ok();
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet("{id}/Questions/{question_sn}")]
        public async Task<ActionResult<ViewModel.QuestionResponse>> GetQuestionSetQuestion(Guid id, int question_sn)
        {
            try
            {
                var questions = await _questionRepository.GetfromQuestionSet(id);
                var question = questions.FirstOrDefault(x => x.Sn == question_sn);
                var questionview=_mapper.Map<QuestionResponse>(question);
                return Ok(questionview);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestionSet(Guid id)
        {
            try
            {
                _questionSetRepository.RemoveAsync(id);
                var isremoved=await _unitOfWork.Commit();
                if(isremoved == false)
                {
                    return BadRequest
                        ($"Failed to delete QuestionSet of Id = {id}");
                }
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestionSet
            (Guid id, [FromBody] QuestionSetForm form)
        {
            try
            {
                var questionset = await _questionSetRepository.GetById(id);
                _mapper.Map(form, questionset);
                _questionSetRepository.UpdateAsync(questionset);
                var isupdated=await _unitOfWork.Commit();
                if(isupdated == false)
                {
                    return BadRequest
                        ($"Failed to Update QuestionSet of Id = {id}");
                }
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
