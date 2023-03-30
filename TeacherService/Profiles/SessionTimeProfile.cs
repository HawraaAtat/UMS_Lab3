using AutoMapper;
using TeacherService.Domain.Models;
using TeacherService.DTO;

namespace TeacherService.Profiles;

public class SessionTimeProfile:Profile
{
    public SessionTimeProfile()
    {
        CreateMap<TeacherPerCourseDTO, SessionTime>();
    }
}