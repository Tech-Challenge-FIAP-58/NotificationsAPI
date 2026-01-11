using FCG.Notifications.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FCG.Notifications.Services.Consumers
{
	public class PaymentProcessedEventConsumer(ILogger<PaymentProcessedEventConsumer> logger) : IConsumer<PaymentProcessedEvent>
	{
		public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
		{
			if (context.Message.Success)
			{
				logger.LogInformation("Payment Processed Event: Payment ID: {PaymentId} processed successfully with Amount: {Amount}", context.Message.PaymentId, context.Message.Amount);
			}
			else
			{
				logger.LogWarning("Payment Processed Event: Payment ID: {PaymentId} failed to process.", context.Message.PaymentId);
			}

			return Task.CompletedTask;
		}
	}
}
