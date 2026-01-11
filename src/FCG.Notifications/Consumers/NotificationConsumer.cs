using FCG.Notifications.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FCG.Notifications.Consumers
{
	public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<Notification>
	{
		public Task Consume(ConsumeContext<Notification> context)
		{
			logger.LogInformation("Received notification: {NotificationId} - {Message}", context.Message.NotificationId, context.Message.Message);
			return Task.CompletedTask;
		}
	}
}
