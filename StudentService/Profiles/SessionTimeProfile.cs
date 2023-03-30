using AutoMapper;
using TeacherService.DTO;
using UMS_Lab3.Domain.Models;

namespace TeacherService.Profiles;

public class SessionTimeProfile:Profile
{
    public SessionTimeProfile()
    {
        CreateMap<TeacherPerCourseDTO, SessionTime>();
    }
}