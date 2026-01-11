using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FCG.Notifications
{
	public class Worker(ILogger<Worker> logger) : BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				logger.LogInformation("FCG.Notifications Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
