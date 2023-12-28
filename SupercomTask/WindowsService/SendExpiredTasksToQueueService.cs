using SupercomTask.BLL.Interfaces;
using SupercomTask.DTO;
using SupercomTask.Models;

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
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}