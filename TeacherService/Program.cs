using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using TeacherService.Application;
using TeacherService.Application.Entities.Course;
using TeacherService.Application.Service;
using TeacherService.Persistence.Models;
using UMS_Lab3.Infrastructure.Abstraction.EmailServiceAbstraction;
using UMS_Lab3.Infrastructure.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//for the add teacher per course
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// for the mediator
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(GetCoursesQuery).GetTypeInfo().Assembly);

//for the auto mapper
builder.Services.AddAutoMapper(typeof(Program));
//for the repository
builder.Services.AddTransient(typeof(IRepository<>), typeof(RepositoryHelper<>));
//for the database
builder.Services.AddDbContext<teacherContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=teacher;Username=postgres;Password=mysecretpassword"));


//for rabbitmq
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        config.ReceiveEndpoint("user", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<UserCreatedConsumer>(provider);
        });
    }));
});

builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();


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
