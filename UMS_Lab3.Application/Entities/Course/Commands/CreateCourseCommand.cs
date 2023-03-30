using MediatR;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Application.Entities.Course.Commands;

public class CreateCourseCommand:IRequest<string>
{
    public Domain.Models.Course course { get; set; }
    
}