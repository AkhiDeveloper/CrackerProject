using AutoMapper;
using BookManager.API.DTOs;
using BookManager.API.Models;
using BookManager.API.ServiceProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BookManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterManager _chapterManager;
        private readonly IMapper _mapper;

        public ChaptersController(IChapterManager chapterManager, IMapper mapper)
        {
            _chapterManager = chapterManager;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChapterDTO>> GetById(Guid id)
        {
            try
            {
                var chapter = await _chapterManager.GetChapterById(id);
                var dto = _mapper.Map<ChapterDTO>(chapter);
                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll/{bookId}")]
        public async Task<ActionResult<IEnumerable<ChapterDTO>>> GetAll
            (Guid bookId, Guid? parentChapterId = null, int offset = 0, int limit = 10, string orderBy = "", bool isAscending = true)
        {
            try
            {
                var chapters = await _chapterManager.GetAllChapter(bookId, parentChapterId);
                var filtered_chapters = chapters.Skip(offset).Take(limit);
                var dto = _mapper.Map<IEnumerable<ChapterDTO>>(filtered_chapters);
                return Ok(new
                {
                    records = dto,
                    total = chapters.Count(),
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{bookId}")]
        public async Task<ActionResult<ChapterDTO>> CreateChapter(Guid bookId, [FromForm]ChapterForm form, [FromQuery]Guid? chapterId = null)
        {
            try
            {
                var chapter_model = _mapper.Map<Chapter>(form);
                await _chapterManager.CreateChapter(bookId, chapter_model, chapterId: chapterId);
                var chapter_model_saved = await _chapterManager.GetChapterById(chapter_model.Id);
                var chapterDTO = _mapper.Map<ChapterDTO>(chapter_model_saved);
                return Ok(chapterDTO);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{chapterId}")]
        public async Task<ActionResult> DeleteChapter(Guid chapterId)
        {
            try
            {
                await _chapterManager.DeleteChapter(chapterId);
                return Ok();
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPatch("{id}/description/{description}")]
        public async Task<ActionResult<ChapterDTO>> ChangeDescription(Guid id, string description)
        {
            try
            {
                await _chapterManager.ChangeChapterDescription(id, description);
                var chapter_model_saved = await _chapterManager.GetChapterById(id);
                var chapterDTO = _mapper.Map<ChapterDTO>(chapter_model_saved);
                return Ok(chapterDTO);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
