using System.Reflection;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using CourseService.Application.Queries;
using CourseService.Application.Abstraction;
using CourseService.Application.Service;
using CourseService.Persistence;
using UMS_Lab3.Infrastructure.Abstraction.EmailServiceAbstraction;
using UMS_Lab3.Infrastructure.EmailService;
using CourseService.Persistence.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CourseService.DateOnlyConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//for the add teacher per course
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// for the mediator
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(GetCoursesQuery).GetTypeInfo().Assembly);
//for the json 
builder.Services
    .AddControllers();
//for the auto mapper
builder.Services.AddAutoMapper(typeof(Program));
//for the repository
builder.Services.AddTransient(typeof(IRepository<>), typeof(RepositoryHelper<>));
//for the database
builder.Services.AddDbContext<classContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=class;Username=postgres;Password=mysecretpassword"));


builder.Services.AddAuthorization();

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer { token }\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/ums-project-ff23d";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/ums-project-ff23d",
            ValidateAudience = true,
            ValidAudience = "ums-project-ff23d",
            ValidateLifetime = true
        };
    });

builder.Services.AddSwaggerGen();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

//firebase
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

