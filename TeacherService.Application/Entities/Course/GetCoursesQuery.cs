using MediatR;

namespace TeacherService.Application.Entities.Course;

public class GetCoursesQuery : IRequest<List<Domain.Models.Course>>
{

}