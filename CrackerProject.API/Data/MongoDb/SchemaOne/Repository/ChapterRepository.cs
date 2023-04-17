using AutoMapper;
using CrackerProject.API.Data.Interfaces;
using CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using Humanizer;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Repository
{
    public class ChapterRepository : BaseRepository<API.Model.Course.Chapter, Chapter, Guid>, IChapterRepository
    {
        private readonly IMongoCollection<CourseChapter> _coursechapters;
        private readonly IMongoCollection<SubChapter> _subchapters;
        private readonly IMongoCollection<Course> _courses;

        public ChapterRepository(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
            _coursechapters = Context.GetCollection<CourseChapter>
                (nameof(Chapter).Pluralize());
            _subchapters = Context.GetCollection<SubChapter>
                (nameof(Chapter).Pluralize());
            _courses = Context.GetCollection<Course>
                (nameof(Course).Pluralize());
        }

        public async Task AddSubChapter(Guid chapter_id, API.Model.Course.Chapter sub_chapter)
        {
            if (!await IsExist(chapter_id))
                throw new NullReferenceException($"Chapter with id = {chapter_id} is not found.");
            var subchapterdata = _mapper.Map<SubChapter>(sub_chapter);
            subchapterdata.ParentChapterId = chapter_id;
            Context.AddCommand(() => 
            _subchapters.InsertOneAsync(subchapterdata));
        }

        public async Task AddtoCourse(Guid course_id, API.Model.Course.Chapter chapter)
        {
            if (!await _courses.Find(x => x.Id == course_id).AnyAsync())
                throw new NullReferenceException($"Course with id = {course_id} is not found.");
            var chapterdata = _mapper.Map<CourseChapter>(chapter);
            chapterdata.ParentCourseId = course_id;
            Context.AddCommand(() =>
            _coursechapters.InsertOneAsync(chapterdata));
        }

        public async Task<IEnumerable<API.Model.Course.Chapter>> GetChaptersofCourse(Guid course_id)
        {
            var chapterdatacursor = await _coursechapters
                .FindAsync(x => x.ParentCourseId == course_id);
            var chapters = chapterdatacursor.ToList();
            return _mapper.Map<IEnumerable<API.Model.Course.Chapter>>(chapters);
        }

        public async Task<IEnumerable<API.Model.Course.Chapter>> GetChaptersofCourse(Guid course_id, Expression<Func<API.Model.Course.Chapter, bool>> predicate)
        {
            Func<CourseChapter, bool> coursefunc = x => x.ParentCourseId == course_id;
            var predicatedata = _mapper.Map<Expression<Func<CourseChapter, bool>>>(predicate).Compile();
            var chapterdatacursor = await _coursechapters
                .FindAsync(x => coursefunc(x) && predicatedata(x));
            var chapters = chapterdatacursor.ToList();
            return _mapper.Map<IEnumerable<API.Model.Course.Chapter>>(chapters);
        }

        public async Task<IEnumerable<API.Model.Course.Chapter>> GetSubChapters(Guid parentchapter_id)
        {
            Func<SubChapter, bool> parentchapterid_func = x => x.ParentChapterId == parentchapter_id;
            var subchaptercursor = await _subchapters.FindAsync(x => parentchapterid_func(x));
            var chapters = subchaptercursor.ToList();
            return _mapper.Map<IEnumerable<API.Model.Course.Chapter>>(chapters);
        }

        public async Task<IEnumerable<API.Model.Course.Chapter>> GetSubChapters(Guid parentchapter_id, Expression<Func<API.Model.Course.Chapter, bool>> predicate)
        {
            Func<SubChapter, bool> parentchapterid_func = x => x.ParentChapterId == parentchapter_id;
            var predicatedata = _mapper.Map<Expression<Func<SubChapter, bool>>>(predicate).Compile();
            var subchaptercursor = await _subchapters.FindAsync(x => parentchapterid_func(x) && predicatedata(x));
            var chapters = subchaptercursor.ToList();
            return _mapper.Map<IEnumerable<API.Model.Course.Chapter>>(chapters);
        }

        public async Task<bool> IsExistInChapter(Guid chapter_id, Guid parentchapter_id)
        {
            var chaptercursor =  await _subchapters
                .FindAsync(x => x.Id == chapter_id && x.ParentChapterId == parentchapter_id);
            return chaptercursor.Any();

        }

        public async Task<bool> IsExistInCourse(Guid chapter_id, Guid course_id)
        {
            var coursecursor = await _coursechapters
                .FindAsync(x => x.Id == chapter_id && x.ParentCourseId == course_id);
            return coursecursor.Any();
        }
    }
}
