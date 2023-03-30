using MediatR;

namespace EnrollementService.Application.Entities.StudentEnrollment;

public class EnrollStudentCommand : IRequest<string>
{
    public Domain.Models.ClassEnrollment EnrollStudent { get; set; }

}