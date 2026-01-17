using FCG.Notifications.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.RegisterConfigurations();
builder.RegisterMassTransit();

var host = builder.Build();

host.Run();