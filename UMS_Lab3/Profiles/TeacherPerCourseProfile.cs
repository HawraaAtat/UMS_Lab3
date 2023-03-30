using AutoMapper;
using UMS_Lab3.DTO;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Profiles;

public class TeacherPerCourseProfile:Profile
{
    public TeacherPerCourseProfile()
    {
        CreateMap<TeacherPerCourseDTO, TeacherPerCourse>();
    }
}