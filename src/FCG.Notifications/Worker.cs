using FCG.Notifications.Models;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FCG.Notifications
{
	//public class Worker(ILogger<Worker> logger, IBus bus) : BackgroundService
	//{
	//	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	//	{
	//		while (!stoppingToken.IsCancellationRequested)
	//		{
	//			await bus.Publish(new Notification
	//			{
	//				NotificationId = Guid.NewGuid(),
	//				CreatedAt = DateTime.Now,
	//				Message = "This is a test notification."
	//			}, stoppingToken);

	//			logger.LogInformation("FCG.Notifications Worker running at: {time}", DateTimeOffset.Now);
	//			await Task.Delay(1000, stoppingToken);
	//		}
	//	}
	//}
}
