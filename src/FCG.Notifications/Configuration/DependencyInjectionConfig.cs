using FCG.Notifications.Domain.Configuration;
using FCG.Notifications.Services.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FCG.Notifications.Configuration
{
	public static class DependencyInjectionConfig
	{
		public static void RegisterConfigurations(this HostApplicationBuilder builder)
		{
			var rabbitMqConfigSection = builder.Configuration.GetSection("RabbitMqSettings");
			builder.Services.Configure<RabbitMqSettings>(rabbitMqConfigSection);
		}

		public static void RegisterMassTransit(this HostApplicationBuilder builder)
		{
			var settings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>() 
				?? throw new NullReferenceException("RabbitMqSettings configuration section is missing or invalid.");

			builder.Services.AddMassTransit(x =>
			{
				x.SetKebabCaseEndpointNameFormatter();
				x.SetInMemorySagaRepositoryProvider();

				x.AddConsumer<PaymentProcessedEventConsumer>();
				x.AddConsumer<UseCreatedEventConsumer>();

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
