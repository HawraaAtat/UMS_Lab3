using AutoMapper;
using NpgsqlTypes;
using UMS_Lab3.DTO;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Profiles;

public class CreateCourseProfile : Profile
{
    public CreateCourseProfile()
    {
        CreateMap<CreateCourseDTO, Course>()
            .ForPath(course => course.EnrolmentDateRange,
                options => options.MapFrom(createCourse => new NpgsqlRange<DateOnly>(DateOnly.FromDateTime(createCourse.StartTime), DateOnly.FromDateTime(createCourse.EndTime))));
    }
}