using MediatR;

namespace UMS_Lab3.Application.Entities.Course.Commands;

public class DeleteCourseCommand:IRequest<string>
{
    public long Id { get; set; }
    
}