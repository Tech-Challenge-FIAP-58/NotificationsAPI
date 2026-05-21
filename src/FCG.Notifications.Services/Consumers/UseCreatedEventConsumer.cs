using FCG.Core.Messages.Integration;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace FCG.Notifications.Services.Consumers
{
    public class UseCreatedEventConsumer(ILogger<UseCreatedEventConsumer> logger, HttpClient httpClient) : IConsumer<UserCreatedEvent>
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            logger.LogInformation("E-mail de boas vindas enviado o usuário #{} com e-mail {}",
                context.Message.UserId, context.Message.Email);

            // pedretti
            EmailAEnviar envio = new EmailAEnviar();
            envio.destinatario = context.Message.Email;
            envio.assunto = "Bem-vindo!";
            envio.corpo = "Seja bem-vindo à plataforma. Sua conta foi criada com sucesso.";

            // pedretti
            var json = JsonConvert.SerializeObject(envio);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // aqui ele chamará o /notifications configurado no Kong API Gateway, que por sua vez redireciona para a lambda function configurada na AWS
            var response = await _httpClient.PostAsync("/notifications", content);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            logger.LogInformation("Resposta da Lambda: {Response}", body);
            // fim pedretti
        }
    }
}