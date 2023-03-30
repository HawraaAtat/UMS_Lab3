using MediatR;
using UMS_Lab3.Application.Abstraction;
using UMS_Lab3.Application.Entities.Course.Commands;
using UMS_Lab3.Persistence;

namespace UMS_Lab3.Application.Entities.TeacherPerCourse.Commands;

public class AddTeacherPerCourseCommandHandler:IRequestHandler<AddTeacherPerCourseCommand,string>
{
    private readonly postgresContext _dbContext;

    public AddTeacherPerCourseCommandHandler(postgresContext context)
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