using AutoMapper;
using model = CrackerProject.API.Model.Course;
using data2 = CrackerProject.API.Data.MongoDb.SchemaSecond.Model;


namespace CrackerProject.API.AutoMapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<model.Course, data2.Course>().ReverseMap();
            CreateMap<model.Course, ViewModel.CourseResponse>();
            CreateMap<ViewModel.CourseForm, model.Course>();
        }
    }
}
