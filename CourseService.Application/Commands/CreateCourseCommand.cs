using CourseService.Domain.Models;
using MediatR;

namespace CourseService.Application.Commands;

public class CreateCourseCommand : IRequest<string>
{
    public Course course { get; set; }
    public int tenantId { get; set; }

}