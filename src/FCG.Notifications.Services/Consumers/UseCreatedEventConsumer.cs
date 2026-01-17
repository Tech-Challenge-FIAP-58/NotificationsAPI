using FCG.Core.Messages.Integration;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FCG.Notifications.Services.Consumers
{
	public class UseCreatedEventConsumer(ILogger<UseCreatedEventConsumer> logger) : IConsumer<UserCreatedEvent>
	{
		public Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			logger.LogInformation("E-mail de boas vindas enviado o usuário #{} com e-mail {}", 
				context.Message.UserId, context.Message.Email);

			return Task.CompletedTask;
		}
	}
}
