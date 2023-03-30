using MediatR;
using UMS_Lab3.Application.Abstraction;

namespace UMS_Lab3.Application.Entities.Course.Queries.GetCourses;

public class GetCourseByIdQueryHandler:IRequestHandler<GetCourseByIdQuery,Domain.Models.Course>
{
    private readonly IRepository<Domain.Models.Course> _repository;

    public GetCourseByIdQueryHandler(IRepository<Domain.Models.Course> repository)
    {
        _repository = repository;
    }
    public async Task<Domain.Models.Course> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll().FirstOrDefault(x => x.Id == request.Id);

    }
}