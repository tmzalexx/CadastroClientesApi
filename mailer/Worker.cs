using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using CadastroClienteApi.mailer.Services;
using CadastroClienteApi.mailer.Infrastructure;

namespace CadastroClienteApi.mailer.Mailer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly MailerSendService _mailerSendService;
        private readonly RabbitService _rabbitService;
        private readonly RegistrationRepository _registrationRepository;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, RegistrationRepository registrationRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _registrationRepository =registrationRepository; 

            var uri = _configuration.GetValue<string>("RabbitMQ:Uri");
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri), "RabbitMQ URI cannot be null or empty.");
            }

            var queueName = _configuration.GetValue<string>("RabbitMQ:QueueName");

            _factory = new ConnectionFactory
            {
                Uri = new Uri(uri)
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: new Dictionary<string, object> {
                                    { "x-queue-type", "quorum" }
                                });

            _mailerSendService = new MailerSendService(_configuration, registrationRepository);
            _rabbitService = new RabbitService(_channel, _mailerSendService);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => _rabbitService.StartConsuming(stoppingToken));
        }
    }
}
