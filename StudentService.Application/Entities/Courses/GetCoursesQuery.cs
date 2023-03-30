using MediatR;

namespace StudentService.Application.Entities.Courses;

public class GetCoursesQuery : IRequest<List<Domain.Models.Course>>
{

}