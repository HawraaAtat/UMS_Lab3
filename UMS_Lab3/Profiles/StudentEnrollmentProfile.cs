using AutoMapper;
using UMS_Lab3.DTO;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Profiles;

public class StudentEnrollmentProfile:Profile
{
    public StudentEnrollmentProfile()
    {
        CreateMap<StudentEnrollDTO, ClassEnrollment>();
    }
}