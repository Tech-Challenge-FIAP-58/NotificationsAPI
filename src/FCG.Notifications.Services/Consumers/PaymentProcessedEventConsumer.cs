using FCG.Core.Integration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;

namespace FCG.Notifications.Services.Consumers
{
    public class PaymentProcessedEventConsumer(
        ILogger<PaymentProcessedEventConsumer> logger,
        IConfiguration configuration) : IConsumer<PaymentProcessedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            EmailAEnviar envio = new EmailAEnviar();

            if (context.Message.Status == PaymentResultStatus.Approved)
            {
                logger.LogInformation("Payment Processed Event: Payment ID: {PaymentId} processed successfully with Amount: {Amount}", context.Message.PaymentId, context.Message.Amount);
                envio.assunto = "Pagamento realizado com Sucesso.";
                envio.corpo = "Parabéns, seu pagamento referente ao pedido " + context.Message.OrderId.ToString() + " foi ralizado com sucesso.";
            }
            else
            {
                logger.LogWarning("Payment Processed Event: Payment ID: {PaymentId} failed to process.", context.Message.PaymentId);
                envio.assunto = "Problemas no seu pagamento.";
                envio.corpo = "Infelizmente tivemos um problema no seu pagamento referente ao pedido " + context.Message.OrderId.ToString() + ". Tente novamente mais tarde.";
            }

            envio.destinatario = "teste@teste.com.br";

            var notificationsUrl = configuration["Notifications__CallbackUrl"]
                                ?? configuration["Notifications:CallbackUrl"];

            if (string.IsNullOrWhiteSpace(notificationsUrl))
            {
                logger.LogInformation("[SIMULADO] Email enviado. Para: {Destinatario}, Assunto: {Assunto}", envio.destinatario, envio.assunto);
                return;
            }

            var json = JsonConvert.SerializeObject(envio);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient { BaseAddress = new Uri(notificationsUrl) };
            var response = await httpClient.PostAsync("/notifications", content);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            logger.LogInformation("Resposta da Lambda: {Response}", body);
        }
    }
}