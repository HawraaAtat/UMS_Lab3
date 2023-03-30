using Firebase.Auth;
using MediatR;
using UMS_Lab3.Application.Abstraction;
using UMS_Lab3.Application.Entities.Course.Queries.GetCourses;
using UMS_Lab3.Domain.Models;
using UMS_Lab3.Persistence;
using User = UMS_Lab3.Domain.Models.User;

namespace UMS_Lab3.Application.Entities.Students.Queries.GetStudentsByCourseID;

public class GetStudentByCourseIdQueryHandler:IRequestHandler<GetStudentByCourseIdQuery,List<User>>
{
    private readonly IRepository<User> _repository;
    private readonly postgresContext _dbContext;
    public GetStudentByCourseIdQueryHandler(IRepository<User> repository, postgresContext context)
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