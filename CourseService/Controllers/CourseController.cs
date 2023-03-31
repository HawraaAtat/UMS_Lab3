using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseService.DTO;
using CourseService.Application.Queries;
using CourseService.Application.Commands;
using CourseService.Domain.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Controllers;


//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class CourseController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CourseController> _logger;

    public CourseController(IMediator mediator,IMapper mapper,ILogger<CourseController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet("GetAllCourses")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Get All Courses execute");
        var result = await _mediator.Send(new GetCoursesQuery());
        
        return Ok(result);
    } 
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById([FromRoute] int id)
    {
        
        var result = await _mediator.Send(new GetCourseByIdQuery
        {
            Id = id
        });
        
        return Ok(result);
    }


    ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[HttpPost("AddCourse")]
    //public async Task<IActionResult> Add([FromBody] CreateCourseDTO c)
    //{
    //    //TODO: Use AutoMapper for mappings
    //    Course course = _mapper.Map<Course>(c);

    //    var result = await _mediator.Send(new CreateCourseCommand
    //    {
    //        course= course
    //    });

    //    return Ok(result);
    //}

    [HttpPost("AddCourse")]
    public async Task<IActionResult> Add([FromBody] CreateCourseDTO c)
    {
        //TODO: Use AutoMapper for mappings
        Course course = _mapper.Map<Course>(c);

        // Get the tenant id from the request headers or somewhere else
        int tenantId = 1; // Replace with the actual tenant id


        var result = await _mediator.Send(new CreateCourseCommand
        {
            course = course,
            tenantId = tenantId
        }) ;

        return Ok(result);
    }


    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateCourseDTO c)
    {
        //TODO: Use AutoMapper for mappings
        Course course = _mapper.Map<Course>(c);

        var existCourse = await _mediator.Send(new GetCourseByIdQuery()
        {
            Id = c.Id
        });

        if (existCourse == null)
        {
            return BadRequest($"No course found with the id {c.Id}");
        }

        
        var result = await _mediator.Send(new UpdateCourseCommand()
        {
            course = course
        });

        return Ok(result);
    }
    
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        var existCourse = await _mediator.Send(new GetCourseByIdQuery()
        {
            Id = id
        });

        if (existCourse == null)
        {
            return BadRequest($"No course found with the id {id}");
        }
        
        var result = await _mediator.Send(new DeleteCourseCommand()
        {
            Id = id
        });

        return Ok(result);
    }
    
}