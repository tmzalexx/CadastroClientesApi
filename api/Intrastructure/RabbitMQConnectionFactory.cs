using RabbitMQ.Client;

public class RabbitMQConnectionFactory
{
    private readonly List<string> _connectionStrings;

    public RabbitMQConnectionFactory(List<string> connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public IConnection CreateConnection(int index)
    {
        var connectionString = _connectionStrings[index];
        var uri = new Uri(connectionString);
        var factory = new ConnectionFactory()
        {
            HostName = uri.Host,
            Port = uri.Port,
            UserName = uri.UserInfo.Split(':')[0],
            Password = uri.UserInfo.Split(':')[1],
            VirtualHost = uri.AbsolutePath.Trim('/')
        };
        return factory.CreateConnection();
    }
}