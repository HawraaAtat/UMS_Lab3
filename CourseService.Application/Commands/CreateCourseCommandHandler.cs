using CourseService.Application.Abstraction;
using CourseService.Domain.Models;
using MediatR;
namespace CourseService.Application.Commands;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, string>
{
    private readonly IRepository<Course> _repository;

    public CreateCourseCommandHandler(IRepository<Course> repository)
    {
        _repository = repository;
    }
    public async Task<string> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        return await _repository.AddAsync(request.course);

    }
}