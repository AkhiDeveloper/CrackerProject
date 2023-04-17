using CrackerProject.API.Model.Course;
using System.Linq.Expressions;

namespace CrackerProject.API.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task AssignCategorytoCourse(Guid course_id, Guid category_id);
        Task<IEnumerable<Category>> GetCourseCategories(Guid course_id);
        Task UnassignfromCourse(Guid course_id, Guid category_id);

        Task AssigntoCategory(Guid id, Guid parentcatergory_id);
        Task UnassigntoCategory(Guid id, Guid parentcatergory_id);
        Task<IEnumerable<Category>> GetSubCategories(Guid parentcategory_id);
        Task<IEnumerable<Category>> GetParentCategories(Guid category_id);

        Task AddMainCategory(Category category);
        Task<IEnumerable<Category>> GetMainCategories();
        Task<IEnumerable<Category>> GetMainCategories(Expression<Func<Category, bool>> predicate);
    }
}
