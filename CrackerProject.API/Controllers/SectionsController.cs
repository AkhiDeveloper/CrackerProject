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
    public class SectionsController : ControllerBase
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IQuestionSetRepository _questionSetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SectionsController(
            ISectionRepository booksectionRepository,
            IQuestionSetRepository questionSetRepository,
            IUnitOfWork unitofWork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _sectionRepository = booksectionRepository;
            _questionSetRepository = questionSetRepository;
            _unitOfWork = unitofWork;
            _mapper = mapper;
        }

        [HttpGet("{id}/sections")]
        public async Task<ActionResult<IEnumerable<SectionResponse>>> GetSections(Guid id)
        {
            try
            {
                //Finding sections
                var subsections = await _sectionRepository.GetfromSection(id);

                //Creating Response
                var response = _mapper.Map<IEnumerable<SectionResponse>>(subsections);
                return Ok(response);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }


        [HttpPost("{id}/section")]
        public async Task<ActionResult> PostSection(Guid id, [FromBody] SectionCreationForm form)
        {
            try
            {
                var newsubsection = _mapper.Map<Section>(form);
                await _sectionRepository.AddtoSection(id, newsubsection);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSection(Guid id)
        {
            try
            {
                //Removing Section
                _sectionRepository.RemoveAsync(id);
                await _sectionRepository.RemoveSubSections(id, DeleteType.AssociatedAlso);
                var success = await _unitOfWork.Commit();
                if (success == false)
                {
                    return BadRequest($"Failed to delete book with {id}");
                }

                //Creating Response
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSection(Guid id, [FromBody] SectionCreationForm form)
        {
            try
            {
                //Checking Section
                if (await _sectionRepository.IsExist(id) == false)
                {
                    return BadRequest($"Section with id = {id} is not found.");
                }

                //Updating Section
                var section = await _sectionRepository.GetById(id);
                _mapper.Map(form, section);
                _sectionRepository.Update(section);

                //Saving Updates
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest($"Failed to update.");
                }

                //Success Response
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }

        [HttpPost("{id}/QuestionSet")]
        public async Task<ActionResult> PostQuestionSet(Guid id, [FromBody] QuestionSetForm form)
        {
            try
            {
                var questionSet = _mapper.Map<QuestionSet>(form);
                await _questionSetRepository.AddtoSection(id, questionSet);
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest("Failed to save question set.");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("{id}/QuestionSets")]
        public async Task<ActionResult<IEnumerable<QuestionSetResponse>>> GetQuestionSets(Guid id)
        {
            try
            {
                var questionSets = await _questionSetRepository.GetfromSection(id);
                var response = _mapper.Map<IEnumerable<QuestionSetResponse>>(questionSets);
                return Ok(questionSets);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

        }
    }


}
