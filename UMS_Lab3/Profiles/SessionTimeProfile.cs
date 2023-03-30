using AutoMapper;
using UMS_Lab3.DTO;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Profiles;

public class SessionTimeProfile:Profile
{
    public SessionTimeProfile()
    {
        CreateMap<TeacherPerCourseDTO, SessionTime>();
    }
}