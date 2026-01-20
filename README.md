# 📢 NotificationsAPI

[![.NET](https://img.shields.io/badge/.NET-8%20%2F%2010-blueviolet)](https://dotnet.microsoft.com/)  
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3-orange)](https://www.rabbitmq.com/)  
[![MassTransit](https://img.shields.io/badge/MassTransit-8-green)](https://masstransit-project.com/)

Serviço leve em **.NET** que roda em background, escuta eventos de integração via **MassTransit + RabbitMQ** e processa notificações de forma assíncrona e escalável.

---

## 📖 Visão Geral
- **Host**: `src/FCG.Notifications` → ponto de entrada da aplicação  
- **Domínio**: `src/FCG.Notifications.Domain` → configurações e tipos (ex.: `RabbitMqSettings`)  
- **Consumers / Serviços**: `src/FCG.Notifications.Services` → manipuladores de mensagens  
- **Testes**: `tests` → testes unitários e de consumidores  

---

## ⚙️ Estado do Workspace
- Projetos direcionados para: `.NET 8` e `.NET 10`  
- Arquivo ativo no editor: `src/FCG.Notifications.Domain/FCG.Notifications.Domain.csproj`  
- Repositório: `https://github.com/Tech-Challenge-FIAP-58/NotificationsAPI` (branch `main`)  

> 💡 **Nota:** ajuste o SDK alvo ao executar ou publicar se algum projeto exigir `.NET 10`. A maioria usa `.NET 8`.

---

## 🛠️ Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) (e 10.0 se necessário)  
- [Docker](https://www.docker.com/) (recomendado para executar RabbitMQ localmente)  
- Visual Studio 2022/2026 ou VS Code  

---

## 🚀 Início Rápido (Docker + Execução Local)
```bash
# 1. Subir RabbitMQ com interface de gerenciamento
docker run -d --hostname rabbit --name rabbitmq \
  -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# 2. Criar arquivo de configuração
# src/FCG.Notifications/appsettings.Development.json
{
  "RabbitMQ": {
    "Host": "localhost",
    "VirtualHost": "/",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

# 3. Executar aplicação
cd src/FCG.Notifications
dotnet run

## 🌍 Variáveis de Ambiente
Suporte via convenção de configuração do .NET:

- `RabbitMQ__Host`  
- `RabbitMQ__VirtualHost`  
- `RabbitMQ__UserName`  
- `RabbitMQ__Password`  

---

## 🔄 Como Funciona
- `Program.cs` registra configurações e MassTransit via `DependencyInjectionConfig`.  
- `DependencyInjectionConfig` lê seção `RabbitMQ` e registra consumidores:  
  - `UseCreatedEventConsumer`  
  - `PaymentProcessedEventConsumer`  
- Mensagens publicadas no RabbitMQ são roteadas automaticamente para os consumidores.

📌 Arquivos relevantes:
- `src/FCG.Notifications/Configuration/DependencyInjectionConfig.cs`  
- `src/FCG.Notifications.Domain/Configuration/RabbitMqSettings.cs`  
- `src/FCG.Notifications.Services/Consumers/UseCreatedEventConsumer.cs`  

---

## 📬 Exemplo de Mensagem (JSON)
```json
{
  "eventType": "PaymentProcessed",
  "userId": "12345",
  "amount": 250.00,
  "currency": "BRL",
  "timestamp": "2026-01-19T21:41:00Z"
}
```
## 🛡️ Solução de Problemas
- 🔌 Se MassTransit não conectar → verifique credenciais e porta `5672`.  
- 🌐 Interface de gerenciamento RabbitMQ → `http://localhost:15672` (usuário: `guest`, senha: `guest`).  
- 📊 Logs → verifique output da aplicação e UI do RabbitMQ.  

---

## ✅ Boas Práticas
- Sempre garantir configuração `RabbitMQ`.  
- Consumidores devem ser **idempotentes**.  
- Tratar falhas e transações de forma robusta.  
- Use **Docker Compose** para orquestrar RabbitMQ + dependências.  

---

## 🤝 Contribuindo
1. Faça um fork do projeto.  
2. Crie uma branch (`git checkout -b minha-feature`).  
3. Commit suas alterações (`git commit -m 'Minha nova feature'`).  
4. Push (`git push origin minha-feature`).  
5. Abra um Pull Request contra `main`.  

---

