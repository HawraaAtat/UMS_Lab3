using CourseService.Application.Abstraction;
using CourseService.Domain.Models;
using MediatR;

namespace CourseService.Application.Commands;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, string>
{
    private readonly IRepository<Course> _repository;

    public DeleteCourseCommandHandler(IRepository<Course> repository)
    {
        _repository = repository;
    }
    public async Task<string> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id);

    }
}