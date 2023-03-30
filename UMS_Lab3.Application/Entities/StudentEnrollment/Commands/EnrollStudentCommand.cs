using MediatR;

namespace UMS_Lab3.Application.Entities.StudentEnrollment.Commands;

public class EnrollStudentCommand:IRequest<string>
{
    public Domain.Models.ClassEnrollment EnrollStudent { get; set; }
    
}