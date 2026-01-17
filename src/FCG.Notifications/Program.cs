using FCG.Notifications;
using FCG.Notifications.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.RegisterConfigurations();
builder.RegisterServices();
builder.RegisterMassTransit();

//builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();