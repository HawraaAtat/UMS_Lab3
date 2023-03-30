using MediatR;

namespace CourseService.Application.Queries;

public class GetCoursesQuery : IRequest<List<Domain.Models.Course>>
{

}