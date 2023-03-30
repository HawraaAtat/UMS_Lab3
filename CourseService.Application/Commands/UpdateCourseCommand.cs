using MediatR;
using CourseService.Domain.Models;

namespace CourseService.Application.Commands;

public class UpdateCourseCommand : IRequest<string>
{
    public Course course { get; set; }

}