using AuthenticationService.Domain.Models;
using AuthenticationService.Persistence;
using FirebaseAdmin;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

using Swashbuckle.AspNetCore.Filters;
using System.Text;
using MassTransit;
using AuthenticationService;
//using MassTransit;

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
        //.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//for the auto mapper
builder.Services.AddAutoMapper(typeof(Program));


//for the Odata
builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("v1", GetEdmModel()).Filter().Select().Expand().Count());

//// for the mediator
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddMediatR(typeof(GetCoursesQuery).GetTypeInfo().Assembly);

//for the database
builder.Services.AddDbContext<authContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=auth;Username=postgres;Password=mysecretpassword"));


//for rabbitmq
builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        }
        );
    }));
});

builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();


//for authorization

var mySecretKey = builder.Configuration["JWT:Key"];


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add the JWT bearer token scheme to Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' followed by a space and then your token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };
    options.AddSecurityDefinition("jwt_auth", securityScheme);

    // Make sure Swagger requires a JWT bearer token to access the API
    var securityRequirement = new OpenApiSecurityRequirement
        {
            { securityScheme, new List<string>() }
        };
    options.AddSecurityRequirement(securityRequirement);

    // Add custom operation filter to include authorization header in Swagger
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});



//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        try
//        {
//            options.Authority = "https://localhost:7010/api/"; // The URL of your authentication server
//        }
//        catch (Exception ex)
//        {
//            ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
//            logger.LogError(ex, "Failed to set up JWT bearer authentication");
//            throw;
//        }
//        options.RequireHttpsMetadata = false; // This should be set to true in production
//        options.Audience = "api1"; // The audience of your API
//    });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MyAPI",
            ValidAudience = "My Audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C1CF4B7DC4C4175B6618DE4F55CA4"))
        };
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireClaim("role", "Admin"));
});


//builder.Services.AddSingleton(FirebaseApp.Create());


//builder.Services.AddAuthorization();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        Description = "Standard Authorization header using the Bearer scheme (\"bearer { token }\")",
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});


//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(o => {
//    o.RequireHttpsMetadata = false;
//    o.SaveToken = false;
//        o.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ClockSkew = TimeSpan.Zero,
//            ValidIssuer = "my issuer",
//            ValidAudience = "my audience",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

//        };


//    });




//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

//app.Use(async (context, next) =>
//{
//    var user = context.User.Identity;
//    if (user != null && user.IsAuthenticated)
//    {
//        Console.WriteLine($"User: {user.Name}");
//        foreach (var claim in context.User.Claims)
//        {
//            Console.WriteLine($"Claim: {claim.Type}={claim.Value}");
//        }
//    }
//    await next.Invoke();
//});

app.Run();


public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                    || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        if (hasAuthorizeAttribute)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "jwt_auth"
                            }
                        },
                        new List<string>()
                    }
                }
            };
        }
    }
}