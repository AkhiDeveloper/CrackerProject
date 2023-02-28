using CrackerProject.API.Model.Course;

namespace CrackerProject.API.Interfaces
{
    public interface ICourseRepository : IRepository<Course, Guid>
    {
        Task<IEnumerable<Course>> GetCoursesofCategory(Guid categoryId);
    }
}
