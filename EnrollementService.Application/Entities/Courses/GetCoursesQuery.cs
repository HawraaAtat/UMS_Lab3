using MediatR;

namespace EnrollementService.Application.Entities.Courses;

public class GetCoursesQuery : IRequest<List<Domain.Models.Course>>
{

}