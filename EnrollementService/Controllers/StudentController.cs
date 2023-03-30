using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EnrollementService.Application.Entities.StudentEnrollment;
using EnrollementService.Application.Entities.Students;
using EnrollementService.Domain.Models;
using EnrollementService.DTO;

namespace EnrollementService.Controllers;
[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StudentController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;

    }
    [HttpGet("Course/{id}")]
    public async Task<IActionResult> GetAllStudentsPerCourse([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetStudentByCourseIdQuery
        {
            Id = id
        });

        return Ok(result);
    }

    [HttpPost("Course/Enrollment")]
    public async Task<IActionResult> Add([FromBody] StudentEnrollDTO e)
    {
        ClassEnrollment enrollment = _mapper.Map<ClassEnrollment>(e);

        var result = await _mediator.Send(new EnrollStudentCommand()
        {
            EnrollStudent = enrollment
        });

        return Ok(result);

    }

}