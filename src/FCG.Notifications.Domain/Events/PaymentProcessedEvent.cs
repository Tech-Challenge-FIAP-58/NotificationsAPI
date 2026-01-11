
namespace FCG.Notifications.Domain.Events
{
	public class PaymentProcessedEvent
	{
		public required Guid PaymentId { get; set; }
		public required decimal Amount { get; set; }
		public required bool Success { get; set; }
	}
}
