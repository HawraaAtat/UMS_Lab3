using MediatR;

namespace StudentService.Application.Entities.Students;

public class GetStudentByCourseIdQuery : IRequest<List<Domain.Models.User>>
{
    public int Id { get; set; }
}