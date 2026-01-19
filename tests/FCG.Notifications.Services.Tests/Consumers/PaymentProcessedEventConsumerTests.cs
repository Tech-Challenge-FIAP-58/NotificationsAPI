using FCG.Core.Messages.Integration;
using FCG.Notifications.Services.Consumers;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.Notifications.Services.Tests.Consumers;

public class PaymentProcessedEventConsumerTests
{
	private readonly Mock<ILogger<PaymentProcessedEventConsumer>> _loggerMock;
	private readonly PaymentProcessedEventConsumer _consumer;

	public PaymentProcessedEventConsumerTests()
	{
		_loggerMock = new Mock<ILogger<PaymentProcessedEventConsumer>>();
		_consumer = new PaymentProcessedEventConsumer(_loggerMock.Object);
	}

	[Fact]
	public async Task Consume_WhenPaymentIsApproved_ShouldLogInformation()
	{
		// Arrange
		var paymentEvent = new PaymentProcessedEvent(
			orderId: 123,
			paymentId: 456,
			amount: 100.50m,
			status: PaymentResultStatus.Approved
		);

		var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
		contextMock.Setup(x => x.Message).Returns(paymentEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Information,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Payment ID: 456") && o.ToString()!.Contains("Amount: 100.5")),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_WhenPaymentIsDenied_ShouldLogWarning()
	{
		// Arrange
		var paymentEvent = new PaymentProcessedEvent(
			orderId: 123,
			paymentId: 789,
			amount: 250.75m,
			status: PaymentResultStatus.Denied,
			reason: "Insufficient funds"
		);

		var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
		contextMock.Setup(x => x.Message).Returns(paymentEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Payment ID: 789") && o.ToString()!.Contains("failed to process")),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_WhenPaymentIsApproved_ShouldNotLogWarning()
	{
		// Arrange
		var paymentEvent = new PaymentProcessedEvent(
			orderId: 100,
			paymentId: 200,
			amount: 50.00m,
			status: PaymentResultStatus.Approved
		);

		var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
		contextMock.Setup(x => x.Message).Returns(paymentEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Never);
	}

	[Fact]
	public async Task Consume_ShouldCompleteSuccessfully()
	{
		// Arrange
		var paymentEvent = new PaymentProcessedEvent(
			orderId: 1,
			paymentId: 2,
			amount: 99.99m,
			status: PaymentResultStatus.Approved
		);

		var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
		contextMock.Setup(x => x.Message).Returns(paymentEvent);

		// Act
		var act = async () => await _consumer.Consume(contextMock.Object);

		// Assert
		await act.Should().NotThrowAsync();
	}

	[Theory]
	[InlineData(PaymentResultStatus.Approved, LogLevel.Information)]
	[InlineData(PaymentResultStatus.Denied, LogLevel.Warning)]
	public async Task Consume_ShouldLogCorrectLevelBasedOnStatus(PaymentResultStatus status, LogLevel expectedLogLevel)
	{
		// Arrange
		var paymentEvent = new PaymentProcessedEvent(
			orderId: 555,
			paymentId: 666,
			amount: 150.00m,
			status: status
		);

		var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
		contextMock.Setup(x => x.Message).Returns(paymentEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				expectedLogLevel,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}
}
