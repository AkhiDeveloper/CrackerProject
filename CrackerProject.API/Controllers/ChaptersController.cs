using AutoMapper;
using CrackerProject.API.Data.Interfaces;
using CrackerProject.API.Model.Course;
using CrackerProject.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrackerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChapterRepository _chapterRepo;

        public ChaptersController(IMapper mapper, IUnitOfWork unitOfWork, IChapterRepository chapterRepo)
        {
            _chapterRepo = chapterRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateChapter(Guid id, [FromBody] ChapterForm form)
        {
            try
            {
                var chapter = await _chapterRepo.GetById(id);
                _mapper.Map(form, chapter);
                _chapterRepo.UpdateAsync(chapter);
                var result = await _unitOfWork.Commit();
                if (!result) return BadRequest("Failed to Save data.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChapter(Guid id)
        {
            try
            {
                _chapterRepo.RemoveAsync(id);
                var result = await _unitOfWork.Commit();
                if (!result) return BadRequest("Failed to Save data.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Chapters")]
        public async Task<ActionResult> PostSubChapter(Guid id, [FromBody] ChapterForm form)
        {
            try
            {
                var subchapter = _mapper.Map<Chapter>(form);
                await _chapterRepo.AddSubChapter(id, subchapter);
                var result = await _unitOfWork.Commit();
                if (!result) return BadRequest("Failed to Save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/Chapters")]
        public async Task<ActionResult<IEnumerable<ChapterResponse>>> GetSubChapters(Guid id)
        {
            try
            {
                var subchapters = await _chapterRepo.GetSubChapters(id);
                return Ok(_mapper.Map<IEnumerable<ChapterResponse>>(subchapters));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

    }
}
