using FCG.Core.Integration;
using FCG.Notifications.Services.Consumers;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.Notifications.Services.Tests.Consumers;

public class PaymentProcessedEventConsumerTests
{
	private readonly Mock<ILogger<PaymentProcessedEventConsumer>> _loggerMock;
	private readonly Mock<IConfiguration> _configurationMock;
	private readonly PaymentProcessedEventConsumer _consumer;

	public PaymentProcessedEventConsumerTests()
	{
		_loggerMock = new Mock<ILogger<PaymentProcessedEventConsumer>>();
		_configurationMock = new Mock<IConfiguration>();
		_consumer = new PaymentProcessedEventConsumer(_loggerMock.Object, _configurationMock.Object);
	}

	[Fact]
	public async Task Consume_WhenPaymentIsApproved_ShouldLogInformation()
	{
		var paymentId = Guid.NewGuid();
		var orderId = Guid.NewGuid();

        // Arrange
        var paymentEvent = new PaymentProcessedEvent(
			orderId: orderId,
			paymentId: paymentId,
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
				It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains($"Payment ID: {paymentId}") && o.ToString()!.Contains("Amount: 100.5")),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_WhenPaymentIsDenied_ShouldLogWarning()
	{
        var paymentId = Guid.NewGuid();
		var orderId = Guid.NewGuid();
        // Arrange
        var paymentEvent = new PaymentProcessedEvent(
			orderId: orderId,
			paymentId: paymentId,
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
				It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains($"Payment ID: {paymentId}") && o.ToString()!.Contains("failed to process")),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_WhenPaymentIsApproved_ShouldNotLogWarning()
	{
        var paymentId = Guid.NewGuid();
		var orderId = Guid.NewGuid();

        // Arrange
        var paymentEvent = new PaymentProcessedEvent(
			orderId: orderId,
			paymentId: paymentId,
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
        var paymentId = Guid.NewGuid();
		var orderId = Guid.NewGuid();
        // Arrange
        var paymentEvent = new PaymentProcessedEvent(
			orderId: orderId,
			paymentId: paymentId,
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
        var paymentId = Guid.NewGuid();
		var orderId = Guid.NewGuid();

        // Arrange
        var paymentEvent = new PaymentProcessedEvent(
			orderId: orderId,
			paymentId: paymentId,
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
			Times.AtLeastOnce);
	}
}
