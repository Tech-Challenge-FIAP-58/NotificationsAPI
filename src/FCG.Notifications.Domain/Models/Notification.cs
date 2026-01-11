
namespace FCG.Notifications.Models
{
	public class Notification
	{
		public required Guid NotificationId { get; set; }
		public required DateTime CreatedAt { get; set; }
		public required string Message { get; set; }
	}
}
