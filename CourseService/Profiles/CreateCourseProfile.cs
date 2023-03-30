using AutoMapper;
using CourseService.Domain.Models;
using CourseService.DTO;
using NpgsqlTypes;

namespace CourseService.Profiles;

public class CreateCourseProfile : Profile
{
    public CreateCourseProfile()
    {
        CreateMap<CreateCourseDTO, Course>()
            .ForPath(course => course.EnrolmentDateRange,
                options => options.MapFrom(createCourse => new NpgsqlRange<DateOnly>(DateOnly.FromDateTime(createCourse.StartTime), DateOnly.FromDateTime(createCourse.EndTime))));
    }
}