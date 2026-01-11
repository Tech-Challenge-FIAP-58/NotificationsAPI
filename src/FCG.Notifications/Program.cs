using FCG.Notifications;
using FCG.Notifications.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.RegisterServices();

var host = builder.Build();

host.Run();