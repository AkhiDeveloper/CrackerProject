using AutoMapper;
using CrackerProject.API.Data.Interfaces;
using CrackerProject.API.Data.MongoDb.SchemaSecond.Model;
using Humanizer;
using MongoDB.Driver;
using ServiceStack;
using System.Linq.Expressions;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Repository
{
    public class CategoryRepository 
        : BaseRepository<API.Model.Course.Category, CourseCategory, Guid>, 
        ICategoryRepository
    {
        private readonly IMongoCollection<SubCategory> _subCategories;
        private readonly IMongoCollection<CourseCategory> _coursecategories;
        private readonly IMongoCollection<MainCategory> _mainCategories;
        private readonly IMongoCollection<Course> _courses;

        public CategoryRepository(IMongoContext context, IMapper mapper) 
            : base(context, mapper)
        {
            _subCategories = Context.GetCollection<SubCategory>
                (nameof(CourseCategory).Pluralize());
            _courses = Context.GetCollection<Course>
                (nameof(Course).Pluralize());
        }

        public async Task AddMainCategory(API.Model.Course.Category category)
        {
            var maincategorydata = _mapper.Map<MainCategory>(category);
            Context.AddCommand(()=>_mainCategories.InsertOneAsync(maincategorydata));
        }

        public async Task AssignCategorytoCourse(Guid course_id, Guid category_id)
        {
            if (!await _courses.Find(x => x.Id == course_id).AnyAsync())
                throw new NullReferenceException($"Chapter with Id = {course_id} is not found.");
            var categorycursor = await _coursecategories.FindAsync(x => x.Id == category_id);
            var category = await categorycursor.SingleOrDefaultAsync();
            if(category == null)
                throw new NullReferenceException($"Category with Id = {category_id} is not found.");
            category.CourseIds.Add(course_id);
            Context.AddCommand(() => _coursecategories.ReplaceOneAsync(x => x.Id == category_id, category));
        }

        public async Task AssigntoCategory(Guid id, Guid parentcatergory_id)
        {
            if (!await DbSet.Find(x => x.Id == parentcatergory_id).AnyAsync())
                throw new NullReferenceException($"Category with Id = {parentcatergory_id} is not found.");
            var categorycursor = await _subCategories.FindAsync(x => x.Id == id);
            var category = await categorycursor.SingleOrDefaultAsync();
            if (category == null)
                throw new NullReferenceException($"Category with Id = {id} is not found.");
            category.ParentCategoryIds.Add(parentcatergory_id);
            Context.AddCommand(() => _subCategories.ReplaceOneAsync(x => x.Id == id, category));
        }

        public async Task<IEnumerable<API.Model.Course.Category>> GetCourseCategories(Guid course_id)
        {
            var categoriescourser = await _coursecategories
                .FindAsync(x => x.CourseIds
                .Any(x => x == course_id));
            var categories = await categoriescourser
                .ToListAsync();
            return _mapper.Map<IEnumerable<API.Model.Course.Category>>(categories);
        }

        public async Task<IEnumerable<API.Model.Course.Category>> GetMainCategories()
        {
            var maincategoriescursor = await _mainCategories.FindAsync(Builders<MainCategory>.Filter.Empty);
            return _mapper.Map<IEnumerable<API.Model.Course.Category>>(maincategoriescursor.ToList());
        }

        public async Task<IEnumerable<API.Model.Course.Category>> GetMainCategories(Expression<Func<API.Model.Course.Category, bool>> predicate)
        {
            var predicatedata = _mapper.Map<Expression<Func<MainCategory, bool>>>(predicate);
            var maincategoriescursor = await _mainCategories.FindAsync(predicatedata);
            return _mapper.Map<IEnumerable<API.Model.Course.Category>>(maincategoriescursor.ToList());
        }

        public async Task<IEnumerable<API.Model.Course.Category>> GetParentCategories(Guid category_id)
        {
            var categoriescourser = await _subCategories
                .FindAsync(x => x.Id == category_id);
            var categories = await categoriescourser
                .FirstOrDefaultAsync();
            return _mapper.Map<IEnumerable<API.Model.Course.Category>>(categories.ParentCategoryIds);
        }

        public async Task<IEnumerable<API.Model.Course.Category>> GetSubCategories(Guid parentcategory_id)
        {
            var categoriescourser = await _subCategories
                .FindAsync(x => x.ParentCategoryIds
                .Any(x => x == parentcategory_id));
            var categories = await categoriescourser
                .ToListAsync();
            return _mapper.Map<IEnumerable<API.Model.Course.Category>>(categories);
        }

        public async Task UnassignfromCourse(Guid course_id, Guid category_id)
        {
            if (!await _courses.Find(x => x.Id == course_id).AnyAsync())
                throw new NullReferenceException($"Chapter with Id = {course_id} is not found.");
            var categorycursor = await _coursecategories.FindAsync(x => x.Id == category_id);
            var category = await categorycursor.SingleOrDefaultAsync();
            if (category == null)
                throw new NullReferenceException($"Category with Id = {category_id} is not found.");
            category.CourseIds.Remove(course_id);
            Context.AddCommand(() => DbSet.ReplaceOneAsync(x => x.Id == category_id, category));
        }

        public async Task UnassigntoCategory(Guid id, Guid parentcatergory_id)
        {
            if (!await DbSet.Find(x => x.Id == parentcatergory_id).AnyAsync())
                throw new NullReferenceException($"Category with Id = {parentcatergory_id} is not found.");
            var categorycursor = await _subCategories.FindAsync(x => x.Id == id);
            var category = await categorycursor.SingleOrDefaultAsync();
            if (category == null)
                throw new NullReferenceException($"Category with Id = {id} is not found.");
            category.ParentCategoryIds.Remove(parentcatergory_id);
            Context.AddCommand(() => _subCategories.ReplaceOneAsync(x => x.Id == id, category));
        }
    }
}
