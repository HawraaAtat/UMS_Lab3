using MediatR;
using EnrollementService.Application.Abstraction;
using EnrollementService.Domain.Models;
using EnrollementService.Persistence;
using UMS_Lab3.Infrastructure.Abstraction.EmailServiceAbstraction;

namespace EnrollementService.Application.Entities.StudentEnrollment;

public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, string>
{
    private readonly IRepository<Domain.Models.ClassEnrollment> _repository;
    private readonly classenrollmentContext _dbContext;
    private readonly IEmailService _emailService;

    public EnrollStudentCommandHandler(IRepository<Domain.Models.ClassEnrollment> repository, classenrollmentContext context, IEmailService emailService)
    {
        _repository = repository;
        _dbContext = context;
        _emailService = emailService;
    }
    public async Task<string> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        var classEnrollment = _dbContext.ClassEnrollments.FirstOrDefault(ce => ce.ClassId == request.EnrollStudent.ClassId && ce.StudentId == request.EnrollStudent.StudentId);
        if (classEnrollment != null)
        {
            return "Student is already enrolled in this course";
        }


        var course = (from tpc in _dbContext.TeacherPerCourses
                      join c in _dbContext.Courses on tpc.CourseId equals c.Id
                      where tpc.Id == request.EnrollStudent.ClassId
                      select new
                      {
                          courseId = c.Id,
                          courseName = c.Name,
                          maxStudentNum = c.MaxStudentsNumber,
                          dateRange = c.EnrolmentDateRange
                      }).FirstOrDefault();


        if (course == null)
        {
            return "Course not found" + course;
        }

        var user = _dbContext.Users.FirstOrDefault(u => u.Id == request.EnrollStudent.StudentId);

        if (user == null)
        {
            return "Student not found";
        }

        DateOnly? startDate = course?.dateRange?.LowerBound;
        DateOnly? endDate = course?.dateRange?.UpperBound;

        if (startDate > DateOnly.FromDateTime(DateTime.Now) || DateOnly.FromDateTime(DateTime.Now) > endDate)
        {
            return "You can't enroll at this time";
        }

        var maxStudNumberByCourse = _dbContext.ClassEnrollments.Count(t => t.ClassId == request.EnrollStudent.ClassId);

        if (maxStudNumberByCourse >= course?.maxStudentNum)
        {
            return "Class is full";
        }

        var classEnrollmentToAdd = new ClassEnrollment
        {
            ClassId = request.EnrollStudent.ClassId,
            StudentId = request.EnrollStudent.StudentId
        };

        var response = await _repository.AddAsync(classEnrollmentToAdd);
        _emailService.SendEmail(user.Email, user.Email, "Enrollment Successful", $"You have been enrolled successfully to the course {course.courseName}");

        return response;


    }
}