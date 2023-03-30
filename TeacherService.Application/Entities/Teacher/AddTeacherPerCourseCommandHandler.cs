using MediatR;
using TeacherService.Persistence.Models;

namespace TeacherService.Application.Entities.Teacher;

public class AddTeacherPerCourseCommandHandler : IRequestHandler<AddTeacherPerCourseCommand, string>
{
    private readonly teacherContext _dbContext;

    public AddTeacherPerCourseCommandHandler(teacherContext context)
    {
        _dbContext = context;
    }
    public async Task<string> Handle(AddTeacherPerCourseCommand request, CancellationToken cancellationToken)
    {
        _dbContext.Add(request.TeacherPerCourse);
        _dbContext.Add(request.SessionTime);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.Add(new Domain.Models.TeacherPerCoursePerSessionTime()
        {
            TeacherPerCourseId = request.TeacherPerCourse.Id,
            SessionTimeId = request.SessionTime.Id
        });
        await _dbContext.SaveChangesAsync(cancellationToken);

        return "Added Successfully!";

    }
}