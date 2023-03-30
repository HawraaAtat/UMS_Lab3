using AutoMapper;
using TeacherService.Domain.Models;
using TeacherService.DTO;

namespace TeacherService.Profiles;

public class TeacherPerCourseProfile:Profile
{
    public TeacherPerCourseProfile()
    {
        CreateMap<TeacherPerCourseDTO, TeacherPerCourse>();
    }
}