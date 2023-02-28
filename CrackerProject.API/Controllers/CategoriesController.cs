using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model.Course;
using CrackerProject.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrackerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IUnitOfWork _uOW;
        public CategoriesController(IUnitOfWork uOW,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            ICourseRepository courseRepository)
        {
            _uOW = uOW;
            _categoryRepo= categoryRepository;
            _courseRepo = courseRepository;
            _mapper = mapper;
        }

        [HttpGet("main")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetMainCategories()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<CategoryResponse>>(_categoryRepo.GetMainCategories()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("main")]
        public async Task<ActionResult> PostMainCategories([FromBody] CategoryForm form)
        {
            try
            {
                await _categoryRepo.AddMainCategory(_mapper.Map<Category>(form));
                var result = await _uOW.Commit();
                if (result == false)
                    return BadRequest("Failed to save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] CategoryForm form)
        {
            try
            {
                var category = await _categoryRepo.GetById(id);
                _mapper.Map(form, category);
                _categoryRepo.UpdateAsync(category);
                var result = await _uOW.Commit();
                if (result == false)
                    return BadRequest("Failed to save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                _categoryRepo.RemoveAsync(id);
                var result = await _uOW.Commit();
                if (result == false) 
                    return BadRequest("Failed to save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/SubCategories")]
        public async Task<ActionResult> PostSubCategory(Guid id, CategoryForm form)
        {
            try
            {
                var subCategory = _mapper.Map<Category>(form);  
                _categoryRepo.AddAsync(subCategory);
                var result = await _uOW.Commit();
                if (result == false)
                    return BadRequest("Failed to save data.");
                await _categoryRepo.AssigntoCategory(subCategory.Id, id);
                result = await _uOW.Commit();
                if (result == false)
                    return BadRequest("Failed to save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/SubCategories")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetSubCategories(Guid id)
        {
            try
            {
                var subcategories = await _categoryRepo.GetSubCategories(id);
                return Ok(_mapper.Map<CategoryResponse>(subcategories));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/subcategories/{subcategory_id}")]
        public async Task<ActionResult> DeleteSubCategories(Guid id, [FromBody] Guid subcategory_id)
        {
            try
            {
                await _categoryRepo.UnassigntoCategory(subcategory_id, id);
                var result = await _uOW.Commit();
                if (result == false) return BadRequest("Failed to save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpPost("{id}/Courses/{course_id}")]
        public async Task<ActionResult> AddCourse(Guid id, Guid course_id)
        {
            try
            {
                await _categoryRepo.AssignCategorytoCourse(course_id, id);
                var result = await _uOW.Commit();
                if (result == false) return BadRequest("Failed to save data.");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/Courses")]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetCourses(Guid id)
        {
            try
            {
                var courses = await _courseRepo.GetCoursesofCategory(id);
                return Ok(_mapper.Map<IEnumerable<CourseResponse>>(courses));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/Courses/{course_id}")]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> UnassignCourses(Guid id, Guid course_id)
        {
            try
            {
                await _categoryRepo.UnassignfromCourse(course_id, id);
                var result = await _uOW.Commit();
                if (result == false) return BadRequest("Failed to save data.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
