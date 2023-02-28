using CrackerProject.API.Model.Course;
using System.Linq.Expressions;

namespace CrackerProject.API.Interfaces
{
    public interface IChapterRepository:IRepository<Chapter, Guid>
    {
        Task AddtoCourse(Guid course_id, Chapter chapter);
        Task<IEnumerable<Chapter>> GetChaptersofCourse(Guid course_id);
        Task<IEnumerable<Chapter>> GetChaptersofCourse(Guid course_id, Expression<Func<Chapter, bool>> predicate);
        Task<bool> IsExistInCourse(Guid chapter_id, Guid course_id);
        
        Task AddSubChapter(Guid chapter_id, Chapter sub_chapter);
        Task<IEnumerable<Chapter>> GetSubChapters(Guid parentchapter_id);
        Task<IEnumerable<Chapter>> GetSubChapters(Guid parentchapter_id, Expression<Func<Chapter, bool>> predicate);
        Task<bool> IsExistInChapter(Guid chapter_id, Guid parentchapter_id);
    }
}
