using FCG.Notifications.Data;
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
		public static void RegisterServices(this HostApplicationBuilder builder)
		{
			builder.Services.AddDbContext<NotificationContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
		}

		public static void RegisterMassTransit(this HostApplicationBuilder builder)
		{
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
					cfg.Host("localhost", "/", h =>
					{
						h.Username("admin");
						h.Password("admin123");
					});
					cfg.ConfigureEndpoints(context);
				});
				//x.UsingInMemory((context, cfg) =>
				//{
				//	cfg.ConfigureEndpoints(context);
				//});
			});
		}
	}
}
