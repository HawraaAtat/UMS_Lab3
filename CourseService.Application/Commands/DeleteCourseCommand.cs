using MediatR;

namespace CourseService.Application.Commands;

public class DeleteCourseCommand : IRequest<string>
{
    public long Id { get; set; }

}