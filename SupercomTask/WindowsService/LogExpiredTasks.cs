using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupercomTask.BLL.Interfaces;
using SupercomTask.Config;
using SupercomTask.DTO;
using System.Text;

public sealed class LogExpiredTasks : RabbitMQServices
{
    private readonly ILogger<LogExpiredTasks> _logger;

    public LogExpiredTasks(RabbitMqConfiguration rabbitMqConfiguration, ILogger<LogExpiredTasks> logger, IServiceProvider serviceProvider) : base(rabbitMqConfiguration, serviceProvider)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       while (!stoppingToken.IsCancellationRequested)
       {
            try
            {
                ConsumeExpiredCardsQueue();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
       }
    }

    private void ConsumeExpiredCardsQueue()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            CardDTO? cardDTO = JsonConvert.DeserializeObject<CardDTO>(body);
            await LogExpiredCards(cardDTO);
        };

        _channel.BasicConsume(_rabbitMqConfiguration.QueueName, false, consumer);
    }

    private async Task LogExpiredCards(CardDTO? cardDTO)
    {
        if (cardDTO != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IExpirationPublishedBLL expirationPublishedBLL = scope.ServiceProvider.GetRequiredService<IExpirationPublishedBLL>();
                await expirationPublishedBLL.PublishExpiration(cardDTO);
            }
        }
    }
}