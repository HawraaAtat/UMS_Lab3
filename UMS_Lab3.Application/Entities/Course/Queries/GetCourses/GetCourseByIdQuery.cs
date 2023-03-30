using MediatR;

namespace UMS_Lab3.Application.Entities.Course.Queries.GetCourses;

public class GetCourseByIdQuery:IRequest<Domain.Models.Course>
{
    public long Id { get; set; }
    
}