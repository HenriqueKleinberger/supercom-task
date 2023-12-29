using Newtonsoft.Json;
using RabbitMQ.Client;
using SupercomTask.BLL.Interfaces;
using SupercomTask.Config;
using SupercomTask.DTO;
using System.Text;

public sealed class SendExpiredTasksToQueueService : RabbitMQServices
{
    private readonly ILogger<SendExpiredTasksToQueueService> _logger;

    public SendExpiredTasksToQueueService(RabbitMqConfiguration rabbitMqConfiguration, ILogger<SendExpiredTasksToQueueService> logger, IServiceProvider serviceProvider) : base(rabbitMqConfiguration, serviceProvider)
    {
        _logger = logger;
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

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
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
            _channel.QueueDeclare(_rabbitMqConfiguration.QueueName, durable: true, false, false, null);

            foreach (var card in cards)
            {
                var messageBody = JsonConvert.SerializeObject(card);
                var properties = _channel.CreateBasicProperties();
                properties.MessageId = card.Id.ToString();
                _channel.BasicPublish("", _rabbitMqConfiguration.QueueName, properties, Encoding.UTF8.GetBytes(messageBody));

                _logger.LogInformation($"Message published to RabbitMQ: {messageBody}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ");
        }
    }
}