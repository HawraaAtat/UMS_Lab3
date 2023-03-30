using AutoMapper;
using EnrollementService.DTO;
using UMS_Lab3.Domain.Models;

namespace EnrollementService.Profiles;

public class TeacherPerCourseProfile:Profile
{
    public TeacherPerCourseProfile()
    {
        CreateMap<TeacherPerCourseDTO, TeacherPerCourse>();
    }
}