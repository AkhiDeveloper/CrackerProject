using AutoMapper;
using CrackerProject.API.Data.MongoDb.SchemaSecond.Model;
using CrackerProject.API.Interfaces;
using Humanizer;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Repository
{
    public class CourseRepository : BaseRepository<API.Model.Course.Course, Course, Guid>, ICourseRepository
    {
        private readonly IMongoCollection<CourseCategory> _coursecategories;
        public CourseRepository(IMongoContext context, IMapper mapper) 
            : base(context, mapper)
        {
            _coursecategories = Context.GetCollection<CourseCategory>(nameof(CourseCategory).Pluralize());
        }

        public async Task<IEnumerable<API.Model.Course.Course>> GetCoursesofCategory(Guid categoryId)
        {
            var categorynode = await _coursecategories.FindAsync(x => x.Id == categoryId);
            var category = await categorynode.FirstOrDefaultAsync();
            IList<API.Model.Course.Course> courses = new List<API.Model.Course.Course>();
            foreach(var ids in category.CourseIds)
            {
                courses.Add(await GetById(ids));
            }
            return courses;
        }
    }
}
