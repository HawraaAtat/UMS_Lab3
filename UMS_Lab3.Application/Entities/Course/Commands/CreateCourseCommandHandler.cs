using MediatR;
using UMS_Lab3.Application.Abstraction;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Application.Entities.Course.Commands;

public class CreateCourseCommandHandler:IRequestHandler<CreateCourseCommand,string>
{
    private readonly IRepository<Domain.Models.Course> _repository;

    public CreateCourseCommandHandler(IRepository<Domain.Models.Course> repository)
    {
        _repository = repository;
    }
    public async Task<string> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        return await _repository.AddAsync(request.course);
        
    }
}