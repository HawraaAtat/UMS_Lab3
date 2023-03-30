using AutoMapper;
using NpgsqlTypes;
using CourseService.DTO;
using CourseService.Domain.Models;

namespace CourseService.Profiles;

public class UpdateCourseProfile : Profile
{
    public UpdateCourseProfile()
    {
        CreateMap<UpdateCourseDTO, Course>()
            .ForPath(course => course.EnrolmentDateRange,
                options => options.MapFrom(UpdateCourseDTO => new NpgsqlRange<DateOnly>(DateOnly.FromDateTime(UpdateCourseDTO.StartTime), DateOnly.FromDateTime(UpdateCourseDTO.EndTime))));
    }
}