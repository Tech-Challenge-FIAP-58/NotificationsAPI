using FCG.Notifications.Data;
using FCG.Notifications.Domain.Configuration;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace FCG.Notifications.Configuration
{
	public static class DependencyInjectionConfig
	{
		public static void RegisterConfigurations(this HostApplicationBuilder builder)
		{
			var rabbitMqConfigSection = builder.Configuration.GetSection("RabbitMqSettings");
			builder.Services.Configure<RabbitMqSettings>(rabbitMqConfigSection);
		}

		public static void RegisterServices(this HostApplicationBuilder builder)
		{
			builder.Services.AddDbContext<NotificationContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
		}

		public static void RegisterMassTransit(this HostApplicationBuilder builder)
		{
			var settings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>() 
				?? throw new NullReferenceException("RabbitMqSettings configuration section is missing or invalid.");

			builder.Services.AddMassTransit(x =>
			{
				x.SetKebabCaseEndpointNameFormatter();
				x.SetInMemorySagaRepositoryProvider();

				var entryAssembly = Assembly.GetEntryAssembly();

				x.AddConsumers(entryAssembly);
				x.AddSagaStateMachines(entryAssembly);
				x.AddSagas(entryAssembly);
				x.AddActivities(entryAssembly);

				x.UsingRabbitMq((context, cfg) =>
				{
					cfg.Host(settings.Host, settings.VirtualHost, h =>
					{
						h.Username(settings.UserName);
						h.Password(settings.Password);
					});
					cfg.ConfigureEndpoints(context);
				});
			});
		}
	}
}
