using AutoMapper;
using StudentService.Domain.Models;
using StudentService.DTO;

namespace StudentService.Profiles;

public class StudentEnrollmentProfile:Profile
{
    public StudentEnrollmentProfile()
    {
        CreateMap<StudentEnrollDTO, ClassEnrollment>();
    }
}