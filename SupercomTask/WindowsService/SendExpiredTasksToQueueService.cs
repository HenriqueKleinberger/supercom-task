using Newtonsoft.Json;
using RabbitMQ.Client;
using SupercomTask.BLL.Interfaces;
using SupercomTask.DTO;
using SupercomTask.Models;
using System.Text;

public sealed class SendExpiredTasksToQueueService : BackgroundService
{
    private readonly ILogger<SendExpiredTasksToQueueService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SendExpiredTasksToQueueService(ILogger<SendExpiredTasksToQueueService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var cardBLL = scope.ServiceProvider.GetRequiredService<ICardBLL>();
                    List<CardDTO> cards = await cardBLL.GetExpiredUndoneCards();
                    _logger.LogWarning($"cards: {cards.Count}");
                    PublishMessageToRabbitMQ(cards);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    private void PublishMessageToRabbitMQ(List<CardDTO> cards)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var rabbitMqConnection = scope.ServiceProvider.GetRequiredService<IConnection>();

                using (var channel = rabbitMqConnection.CreateModel())
                {
                    channel.QueueDeclare("expired_cards", durable: true, false, false, null);

                    foreach (var card in cards)
                    {
                        var messageBody = JsonConvert.SerializeObject(card);
                        var properties = channel.CreateBasicProperties();
                        properties.MessageId = card.Id.ToString();

                        // Publish the message to the queue
                        channel.BasicPublish("", "expired_cards", properties, Encoding.UTF8.GetBytes(messageBody));

                        _logger.LogInformation($"Message published to RabbitMQ: {messageBody}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ");
            // Log or handle the exception as needed
        }
    }
}