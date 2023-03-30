using MediatR;

namespace UMS_Lab3.Application.Entities.Students.Queries.GetStudentsByCourseID;

public class GetStudentByCourseIdQuery:IRequest<List<Domain.Models.User>>
{
    public int Id { get; set; }
}