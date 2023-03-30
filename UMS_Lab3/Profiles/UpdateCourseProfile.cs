using AutoMapper;
using NpgsqlTypes;
using UMS_Lab3.DTO;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Profiles;

public class UpdateCourseProfile:Profile
{
    public UpdateCourseProfile()
    {
        CreateMap<UpdateCourseDTO, Course>()
            .ForPath(course => course.EnrolmentDateRange,
                options => options.MapFrom(UpdateCourseDTO => new NpgsqlRange<DateOnly>(DateOnly.FromDateTime(UpdateCourseDTO.StartTime), DateOnly.FromDateTime(UpdateCourseDTO.EndTime))));
    }
}