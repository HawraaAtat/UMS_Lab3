using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS_Lab3.DTO;
using UMS_Lab3.Application.Entities.TeacherPerCourse.Commands;
using UMS_Lab3.Domain.Models;


namespace UMS_Lab3.Controllers;

[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TeacherController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("addTeacherPerCourse")]
    public async Task<IActionResult> Add([FromBody] TeacherPerCourseDTO t)
    {
        //TODO: Use AutoMapper for mappings
        TeacherPerCourse TeacherPerCourse = _mapper.Map<TeacherPerCourse>(t);

        SessionTime SessionTime = _mapper.Map<SessionTime>(t);

        var result = await _mediator.Send(new AddTeacherPerCourseCommand()
        {
            TeacherPerCourse = TeacherPerCourse,
            SessionTime = SessionTime,

        });
        return Ok(result);
    }

}