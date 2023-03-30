using MassTransit;
using Microsoft.EntityFrameworkCore;
using TeacherService.Domain.Models;
using TeacherService.Persistence.Models;

namespace TeacherService
{
    public class UserCreatedConsumer : IConsumer<User>
    {
        private readonly teacherContext _dbContext;

        public UserCreatedConsumer(teacherContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<User> context)
        {
            var user = context.Message;
            Console.WriteLine(user);
            // Add the user to the database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Log the user creation event
            Console.WriteLine($"User created: {user.Name}, {user.Email}, {user.KeycloakId}");


        }
    }

}
