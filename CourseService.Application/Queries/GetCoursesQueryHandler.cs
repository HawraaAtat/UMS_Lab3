using MediatR;
using CourseService.Application.Abstraction;

namespace CourseService.Application.Queries;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<Domain.Models.Course>>
{
    private readonly IRepository<Domain.Models.Course> _repository;

    public GetCoursesQueryHandler(IRepository<Domain.Models.Course> repository)
    {
        _repository = repository;
    }

    public async Task<List<Domain.Models.Course>> Handle(GetCoursesQuery
        request, CancellationToken cancellationToken)
    {
        return _repository.GetAll().ToList();
    }
}