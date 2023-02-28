using AutoMapper;
using CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using CrackerProject.API.Interfaces;
using System.Linq.Expressions;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Repository
{
    public class CourseRepository : BaseRepository<API.Model.Course.Course, Course, Guid>, ICourseRepository
    {
        public CourseRepository(IMongoContext context, IMapper mapper) 
            : base(context, mapper)
        {

        }

        public Task<IEnumerable<API.Model.Course.Course>> GetCoursesofCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
