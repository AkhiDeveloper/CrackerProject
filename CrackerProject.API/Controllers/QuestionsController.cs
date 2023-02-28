using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;
using CrackerProject.API.Model.Book;
using CrackerProject.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrackerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageManager _storageManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public QuestionsController(
            IQuestionRepository questionRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IStorageManager storageManager,
            IWebHostEnvironment hostEnvironment)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageManager = storageManager;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("{id}/Options")]
        public async Task<ActionResult> AddOptionSets(Guid id, [FromBody] IEnumerable<OptionForm> options)
        {
            try
            {
                var questionmodel = await _questionRepository.GetById(id);
                var optionmodels = _mapper.Map<IList<Option>>(options);
                if (questionmodel.OptionSets == null)
                {
                    questionmodel.OptionSets = new List<OptionSet>();
                }
                var sn = questionmodel.OptionSets.Count() + 1;
                var optioset = new OptionSet() { Sn = sn, Options = optionmodels };
                questionmodel.OptionSets.Add(optioset);
                _questionRepository.UpdateAsync(questionmodel);
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest("Failed to save data.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Image")]
        public async Task<ActionResult> UploadQuestionImage(IFormFile imagefileform)
        {
            try
            {
                var filename = DateTime.UtcNow.Ticks.ToString() + "_" + imagefileform.FileName;
                var filepath = Path.Combine(_hostEnvironment.ContentRootPath, "Image", filename);
                using(FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    await imagefileform.CopyToAsync(stream);
                    StorageDirectory imagedirectory = new StorageDirectory("Images");
                    await _storageManager.ChangeActiveDirectory(imagedirectory);
                    var filedirectory = _storageManager.UploadFile(stream, new CancellationToken());
                    if(filedirectory == null)
                    {
                        return BadRequest("Failed to Upload Image.");
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(Guid id, [FromBody] QuestionForm form)
        {
            try
            {
                var question = await _questionRepository.GetById(id);
                _mapper.Map(form, question);
                var result = await _unitOfWork.Commit();
                if(result == false)
                {
                    return BadRequest("Failed to save data.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(Guid id)
        {
            try
            {
                _questionRepository.RemoveAsync(id);
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest("Failed to save data.");
                }
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPut("{id}/Options/{optionset_sn}")]
        public async Task<ActionResult> UpdateOptionSets(Guid id, int optionset_sn, [FromBody] IEnumerable<OptionForm> options)
        {
            try
            {
                var question = await _questionRepository.GetById(id);
                var optionset = question.OptionSets.FirstOrDefault(x => x.Sn == optionset_sn);
                if (optionset == null)
                {
                    return BadRequest("Optionset is not found.");
                }
                optionset.Options.Clear();
                var optionmodels = _mapper.Map<IList<Option>>(options);
                foreach (var option in optionmodels)
                {
                    optionset.Options.Add(option);
                }
                _questionRepository.UpdateAsync(question);
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest("Failed to save data.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
