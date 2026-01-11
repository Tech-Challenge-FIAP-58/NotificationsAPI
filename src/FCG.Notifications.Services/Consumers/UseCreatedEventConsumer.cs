using FCG.Notifications.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FCG.Notifications.Services.Consumers
{
	public class UseCreatedEventConsumer(ILogger<UseCreatedEventConsumer> logger) : IConsumer<UserCreatedEvent>
	{
		public Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			logger.LogInformation("User Created Event: E-mail sent successfully");
			return Task.CompletedTask;
		}
	}
}
