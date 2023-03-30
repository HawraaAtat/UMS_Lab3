using EnrollementService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentService.Application.Abstraction;
using StudentService.Application.Entities.Courses;
using StudentService.Application.Service;
using System.Reflection;
using UMS_Lab3.Infrastructure.Abstraction.EmailServiceAbstraction;
using UMS_Lab3.Infrastructure.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for the mediator
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(GetCoursesQuery).GetTypeInfo().Assembly);

//for the auto mapper
builder.Services.AddAutoMapper(typeof(Program));
//for the repository
builder.Services.AddTransient(typeof(IRepository<>), typeof(RepositoryHelper<>));
//for the database
builder.Services.AddDbContext<classenrollmentContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=classenrollment;Username=postgres;Password=mysecretpassword"));

//for the email
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
