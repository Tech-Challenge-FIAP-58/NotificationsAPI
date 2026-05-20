
namespace FCG.Notifications.Domain.Configuration
{
	public class RabbitMqSettings
	{
		public required string Host { get; set; }
		public required string VirtualHost { get; set; }
		public required string UserName { get; set; }
		public required string Password { get; set; }
		public int Port { get; set; } = 5672;
		public bool UseSsl { get; set; } = false;
	}
}
