using MediatR;
using TeacherService.Domain.Models;

namespace TeacherService.Application.Entities.Teacher;

public class AddTeacherPerCourseCommand : IRequest<string>
{
    public TeacherPerCourse TeacherPerCourse { get; set; }
    public SessionTime SessionTime { get; set; }

}