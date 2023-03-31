using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using TeacherService.Domain.Models;
using TeacherService.Persistence.Models;

namespace TeacherService;

public class UserCreatedConsumer : IConsumer<User>
{
    private readonly teacherContext _dbContext;
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(teacherContext dbContext, ILogger<UserCreatedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<User> context)
    {
        //var user = context.Message;

        var userBytes = context.GetPayload<byte[]>();
        // Deserialize the user object
        var user = JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(userBytes));


        // Add the user to the database
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Log the user creation event
        _logger.LogInformation($"User created: {user.Name}, {user.Email}, {user.KeycloakId}");


    }
}
