using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model.Course;
using CrackerProject.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CrackerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IChapterRepository _chapterRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CoursesController(
            ICourseRepository courseRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IChapterRepository chapterRepository)
        {
            _courseRepo = courseRepo;
            _chapterRepo = chapterRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> PostCourse([FromBody] CourseForm form)
        {
            try
            {
                var course = _mapper.Map<Course>(form);
                _courseRepo.AddAsync(course);
                var result = await _unitOfWork.Commit();
                if (!result) return BadRequest("Failed to Save data.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterResponse>>> GetAllCourses()
        {
            try
            {
                var courses = await _courseRepo.GetAll();
                return Ok(_mapper.Map<IEnumerable<ChapterResponse>>(courses));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, [FromBody] CourseForm form)
        {
            try
            {
                var course = await _courseRepo.GetById(id);
                _mapper.Map(form, course);
                _courseRepo.UpdateAsync(course);
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
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            try
            {
                _courseRepo.RemoveAsync(id);
                var result = await _unitOfWork.Commit();
                if (!result) return BadRequest("Failed to Save data.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/Chapters")]
        public async Task<ActionResult<IEnumerable<ChapterResponse>>> GetCourseChapter(Guid id)
        {
            try
            {
                var chapters = await _chapterRepo.GetChaptersofCourse(id);
                return Ok(_mapper.Map<IEnumerable<ChapterResponse>>(chapters));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Chapters")]
        public async Task<ActionResult> PostCourseChapter(Guid id, [FromBody] ChapterForm form)
        {
            try
            {
                var chapter = _mapper.Map<Chapter>(form);
                await _chapterRepo.AddtoCourse(id, chapter);
                var result = await _unitOfWork.Commit();
                if (!result) return BadRequest("Failed to Save data.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
