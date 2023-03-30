using MediatR;

namespace TeacherService.Application.Entities.Course;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<TeacherService.Domain.Models.Course>>
{
    private readonly IRepository<TeacherService.Domain.Models.Course> _repository;

    public GetCoursesQueryHandler(IRepository<TeacherService.Domain.Models.Course> repository)
    {
        _repository = repository;
    }

    public async Task<List< TeacherService.Domain.Models.Course>> Handle(GetCoursesQuery
        request, CancellationToken cancellationToken)
    {
        return _repository.GetAll().ToList();
    }
}