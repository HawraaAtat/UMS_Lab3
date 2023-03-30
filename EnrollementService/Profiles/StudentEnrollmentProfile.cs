using AutoMapper;
using EnrollementService.Domain.Models;
using EnrollementService.DTO;

namespace StudentService.Profiles;

public class StudentEnrollmentProfile:Profile
{
    public StudentEnrollmentProfile()
    {
        CreateMap<StudentEnrollDTO, ClassEnrollment>();
    }
}