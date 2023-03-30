using MediatR;

namespace CourseService.Application.Queries;

public class GetCourseByIdQuery : IRequest<Domain.Models.Course>
{
    public long Id { get; set; }

}