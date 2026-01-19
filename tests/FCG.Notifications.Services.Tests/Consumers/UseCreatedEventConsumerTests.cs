using FCG.Core.Messages.Integration;
using FCG.Notifications.Services.Consumers;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.Notifications.Services.Tests.Consumers;

public class UseCreatedEventConsumerTests
{
	private readonly Mock<ILogger<UseCreatedEventConsumer>> _loggerMock;
	private readonly UseCreatedEventConsumer _consumer;

	public UseCreatedEventConsumerTests()
	{
		_loggerMock = new Mock<ILogger<UseCreatedEventConsumer>>();
		_consumer = new UseCreatedEventConsumer(_loggerMock.Object);
	}

	[Fact]
	public async Task Consume_WhenUserCreated_ShouldLogInformation()
	{
		// Arrange
		var userEvent = new UserCreatedEvent
		{
			UserId = 123,
			Email = "usuario@teste.com"
		};

		var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
		contextMock.Setup(x => x.Message).Returns(userEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Information,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("123") && o.ToString()!.Contains("usuario@teste.com")),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_ShouldLogWelcomeMessageWithCorrectFormat()
	{
		// Arrange
		var userEvent = new UserCreatedEvent
		{
			UserId = 456,
			Email = "novousuario@exemplo.com"
		};

		var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
		contextMock.Setup(x => x.Message).Returns(userEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Information,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("E-mail de boas vindas enviado")),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_ShouldCompleteSuccessfully()
	{
		// Arrange
		var userEvent = new UserCreatedEvent
		{
			UserId = 789,
			Email = "teste@email.com"
		};

		var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
		contextMock.Setup(x => x.Message).Returns(userEvent);

		// Act
		var act = async () => await _consumer.Consume(contextMock.Object);

		// Assert
		await act.Should().NotThrowAsync();
	}

	[Theory]
	[InlineData(1, "user1@test.com")]
	[InlineData(999, "admin@company.com")]
	[InlineData(12345, "long.email.address@subdomain.example.com")]
	public async Task Consume_WithDifferentUserData_ShouldLogCorrectly(int userId, string email)
	{
		// Arrange
		var userEvent = new UserCreatedEvent
		{
			UserId = userId,
			Email = email
		};

		var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
		contextMock.Setup(x => x.Message).Returns(userEvent);

		// Act
		await _consumer.Consume(contextMock.Object);

		// Assert
		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Information,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((o, t) => 
					o.ToString()!.Contains(userId.ToString()) && 
					o.ToString()!.Contains(email)),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Once);
	}

	[Fact]
	public async Task Consume_ShouldNotLogWarningOrError()
	{
		// Arrange
		var userEvent = new UserCreatedEvent
		{
			UserId = 100,
			Email = "normal@user.com"
		};

		var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
		contextMock.Setup(x => x.Message).Returns(userEvent);

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

		_loggerMock.Verify(
			x => x.Log(
				LogLevel.Error,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
			Times.Never);
	}
}
