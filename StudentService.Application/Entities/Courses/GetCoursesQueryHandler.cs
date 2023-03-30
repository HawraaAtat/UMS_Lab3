using MediatR;
using StudentService.Application.Abstraction;
using StudentService.Domain.Models;

namespace StudentService.Application.Entities.Courses;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<Course>>
{
    private readonly IRepository<Course> _repository;

    public GetCoursesQueryHandler(IRepository<Course> repository)
    {
        _repository = repository;
    }

    public async Task<List<Course>> Handle(GetCoursesQuery
        request, CancellationToken cancellationToken)
    {
        return _repository.GetAll().ToList();
    }
}