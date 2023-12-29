using RabbitMQ.Client;
using SupercomTask.Config;

public abstract class RabbitMQServices : BackgroundService
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly RabbitMqConfiguration _rabbitMqConfiguration;
    protected IConnection _connection;
    protected IModel _channel;

    public RabbitMQServices(RabbitMqConfiguration rabbitMqConfiguration, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _rabbitMqConfiguration = rabbitMqConfiguration;
        InitRabbitMQ();
    }

    private void InitRabbitMQ()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqConfiguration.HostName,
            UserName = _rabbitMqConfiguration.UserName,
            Password = _rabbitMqConfiguration.Password
        };


        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_rabbitMqConfiguration.QueueName, true, false, false, null);
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}