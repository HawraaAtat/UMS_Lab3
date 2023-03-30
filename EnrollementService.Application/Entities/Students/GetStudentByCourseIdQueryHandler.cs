using Firebase.Auth;
using MediatR;
using EnrollementService.Application.Abstraction;
using EnrollementService.Persistence;
using User = EnrollementService.Domain.Models.User;

namespace EnrollementService.Application.Entities.Students;

public class GetStudentByCourseIdQueryHandler : IRequestHandler<GetStudentByCourseIdQuery, List<User>>
{
    private readonly IRepository<User> _repository;
    private readonly classenrollmentContext _dbContext;
    public GetStudentByCourseIdQueryHandler(IRepository<User> repository, classenrollmentContext context)
    {
        _repository = repository;
        _dbContext = context;
    }
    public async Task<List<User>> Handle(GetStudentByCourseIdQuery request, CancellationToken cancellationToken)
    {
        var students = (from ep in _dbContext.Courses
                        join e in _dbContext.TeacherPerCourses on ep.Id equals e.CourseId
                        join t in _dbContext.ClassEnrollments on e.Id equals t.ClassId
                        join u in _dbContext.Users on t.StudentId equals u.Id
                        where ep.Id == request.Id
                        select u).ToList();

        return students;


    }
}