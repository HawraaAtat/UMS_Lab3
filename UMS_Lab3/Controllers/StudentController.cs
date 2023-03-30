using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS_Lab3.DTO;
using UMS_Lab3.Application.Entities.StudentEnrollment.Commands;
using UMS_Lab3.Application.Entities.Students.Queries.GetStudentsByCourseID;
using UMS_Lab3.Domain.Models;

namespace UMS_Lab3.Controllers;
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

    [HttpPost("enrollment")]
    public async Task<IActionResult> Add([FromBody] StudentEnrollDTO e)
    {
        //TODO: Use AutoMapper for mappings
        ClassEnrollment enrollment = _mapper.Map<ClassEnrollment>(e);

        var result = await _mediator.Send(new EnrollStudentCommand()
        {
            EnrollStudent = enrollment
        });

        return Ok(result);

    }

}