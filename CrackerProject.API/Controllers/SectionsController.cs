using AutoMapper;
using CrackerProject.API.DataModels;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;
using CrackerProject.API.Repository;
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
        private readonly ISubSectionRepository _subSectionRepository;
        private readonly IQuestionSetRepository _questionSetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionsController(
            ISectionRepository booksectionRepository, 
            IQuestionSetRepository questionSetRepository,
            IUnitOfWork unitofWork, 
            IMapper mapper,
            ISubSectionRepository subSectionRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _sectionRepository = booksectionRepository;
            _subSectionRepository = subSectionRepository;
            _questionSetRepository = questionSetRepository;
            _unitOfWork = unitofWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}/sections")]
        public async Task<ActionResult<IEnumerable<SectionResponse>>> GetSections(Guid id)
        {
            //Finding parent section
            var section = await _sectionRepository.GetById(id);
            if(section == null)
            {
                return NotFound(new
                {
                    message = $"Section with id = {id} not found",
                });
            }

            //Finding sub sections
            var subsections = await _subSectionRepository.Find(x => x.ParentBookSectionId == section.Id);
            if(subsections == null || subsections.Count() < 1)
            {
                return NotFound(new
                {
                    message = $"No Sub-Section is found"
                });
            }

            //Creating Response
            var response = _mapper.Map<IEnumerable<SectionResponse>>(subsections);
            return Ok(new
            {
                success= true,
                response = response,
            });
        }


        [HttpPost("{id}/section")]
        public async Task<ActionResult<SectionResponse>> PostSection(Guid id, [FromBody] SectionCreationForm form)
        {
            //Finding parent section
            var section = await _sectionRepository.GetById(id);
            if (section == null)
            {
                return NotFound(new
                {
                    message = $"Section with id = {id} not found",
                });
            }

            //Creating Sub-Section
            var newsubsection = _mapper.Map<SubSection>(form);
            newsubsection.ParentBookSectionId = section.Id;

            //Saving Sub-Section
            _subSectionRepository.Add(newsubsection);
            var result = await _unitOfWork.Commit();
            if(result == false)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to Save."
                });
            }

            //Creating Response
            var responsemodel = _mapper.Map<SectionResponse>(newsubsection);
            return Ok(new
            {
                result = true,
                response = responsemodel,
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SectionResponse>> DeleteSection(Guid id)
        {
            //Finding section
            var section = await _sectionRepository.GetById(id);
            if (section == null)
            {
                return NotFound(new
                {
                    message = "Section is not found."
                });
            }

            //Removing Section
            _sectionRepository.Remove(id);
            var success = await _unitOfWork.Commit();
            if(success == false)
            {
                return BadRequest(new
                {
                    success = success,
                    message = $"Failed to delete book with {id}"
                });
            }

            //Creating Response
            var response = _mapper.Map<SectionResponse>(section);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSection(Guid id, [FromBody] SectionCreationForm form)
        {
            //Checking Section
            if(await _sectionRepository.IsExist(id) == false)
            {
                return NotFound(new
                {
                    message = "Section is not found."
                });
                
            }

            //Updating Section
            var section = await _sectionRepository.GetById(id);
            _mapper.Map(form, section);
            _sectionRepository.Update(section);

            //Saving Updates
            var result = await _unitOfWork.Commit();
            if(result == false)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"Failed to update."
                });
            }

            //Success Response
            return Ok();
        }

        [HttpPost("{id}/QuestionSet")]
        public async Task<ActionResult> PostQuestionSet(Guid id, [FromBody] QuestionSetForm form)
        {
            try
            {
                //Checking Section
                if(await _sectionRepository.IsExist(id) == false)
                {
                    return NotFound(new
                    {
                        message = "Section is not found."
                    });
                }
                var section = await _sectionRepository.GetById(id);

                //Adding Questionset
                var newquestionset = _mapper.Map<QuestionSet>(form);
                if (section.QuestionSets == null)
                {
                    section.QuestionSets = new List<QuestionSet>();
                    newquestionset.SN = 1;
                }
                newquestionset.SN = section.QuestionSets.Count() + 1;
                section.QuestionSets.Add(newquestionset);
                
                //Saving QuestionSet Updates
                _sectionRepository.Update(section);
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Failed to save question set."
                    });
                }

                //Creating Response
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("{id}/QuestionSets")]
        public async Task<ActionResult<IEnumerable<QuestionSetResponse>>> GetQuestionSets(Guid id)
        {
            //Checking section
            var section = await _sectionRepository.GetById(id);
            if(section == null)
            {
                return NotFound(new
                {
                    message = $"Section is not found."
                });
            }

            //Checking QuestionSet
            if(section.QuestionSets == null || section.QuestionSets.Count()<1)
            {
                return NotFound(new
                {
                    message = "No Question Set is found in this Section"
                });
            }

            //Creating Response
            var response = _mapper.Map<IEnumerable<QuestionSetResponse>>(section.QuestionSets);
            return Ok(response);
        }

        [HttpPut("{id}/QuestionSets/{sn}")]
        public async Task<ActionResult<QuestionSetResponse>> UpdateQuestionSets(Guid id, int sn, [FromBody] QuestionSetForm form)
        {
            try
            {
                //Checking section
                var section = await _sectionRepository.GetById(id);
                if (section == null)
                {
                    return NotFound(new
                    {
                        message = $"Section is not found."
                    });
                }

                //Checking QuestionSet
                if (section.QuestionSets == null || section.QuestionSets.Count() < 1)
                {
                    return NotFound(new
                    {
                        message = "No Question Set is found in this Section"
                    });
                }

                //Updating QuestionSet
                _mapper.Map(form, section.QuestionSets.Where(x => x.SN == sn).FirstOrDefault());

                //Saving
                _sectionRepository.Update(section);
                var result = await _unitOfWork.Commit();
                if (result == false)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Failed to save question set."
                    });
                }

                //Creating Response
                var response = _mapper.Map<QuestionSetResponse>(section.QuestionSets.Where(x => x.SN == sn).FirstOrDefault());
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpDelete("{id}/QuestionSets/{sn}")]
        public async Task<ActionResult<QuestionSetResponse>> DeleteQuestionSets(Guid id, int sn)
        {
            //Checking section
            var section = await _sectionRepository.GetById(id);
            if (section == null)
            {
                return NotFound(new
                {
                    message = $"Section is not found."
                });
            }

            //Checking QuestionSet
            if (section.QuestionSets == null || section.QuestionSets.Count() < 1)
            {
                return NotFound(new
                {
                    message = "No Question Set is found in this Section"
                });
            }
            if(!section.QuestionSets.Any(x=>x.SN==sn))
            {
                return NotFound(new
                {
                    message = $"No Question Set with \bSN = \b{sn} is found"
                });
            }

            //Deleting QuestionSet
            var questionset=section.QuestionSets.FirstOrDefault(x=>x.SN==sn);
            section.QuestionSets.Remove(questionset);

            //Saving
            _sectionRepository.Update(section);
            var result = await _unitOfWork.Commit();
            if (result == false)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to save question set."
                });
            }

            //Creating Response
            return Ok();
        }

        [HttpPost("{id}/QuestionSets/{sn}/Question/Objective")]
        public async Task<ActionResult<QuestionResponse>> PostObjectiveQuestion(Guid id, int sn, [FromForm] ObjectiveQuestionForm form)
        {
            //Checking section
            var section = await _sectionRepository.GetById(id);
            if (section == null)
            {
                return NotFound(new
                {
                    message = $"Section is not found."
                });
            }

            //Checking QuestionSet
            if (section.QuestionSets == null || section.QuestionSets.Count() < 1)
            {
                return NotFound(new
                {
                    message = "No Question Set is found in this Section"
                });
            }

            //Uploading Pictures


            //Adding Question
            var question = _mapper.Map<ObjectiveQuestion>(form);
            var questionset = section.QuestionSets.FirstOrDefault(x => x.SN == sn);
            if(questionset == null)
            {
                return NotFound(new
                {
                    message = $"No Question Set with \bSN = \b{sn} is found"
                });
            }
            questionset.Questions.Add(question);

            //Saving
            _sectionRepository.Update(section);
            var result = await _unitOfWork.Commit();
            if (result == false)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to save question set."
                });
            }

            //Creating Response
            var response = _mapper.Map<QuestionResponse>(question);
            return Ok(response);
        }
    }

    
}
