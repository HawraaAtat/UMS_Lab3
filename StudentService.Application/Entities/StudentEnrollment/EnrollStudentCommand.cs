using MediatR;

namespace StudentService.Application.Entities.StudentEnrollment;

public class EnrollStudentCommand : IRequest<string>
{
    public Domain.Models.ClassEnrollment EnrollStudent { get; set; }

}