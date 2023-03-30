// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Builder;
using MassTransit;
using TeacherService.Application;

Console.WriteLine("Hello, World!");


var builder = WebApplication.CreateBuilder(args);
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



var app = builder.Build();

app.Run();