using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CadastroClienteApi.mailer.Models;
using Microsoft.Extensions.Configuration;

namespace CadastroClienteApi.mailer.Services
{
    public class RabbitService
    {
        private readonly IModel _channel;
        private readonly MailerSendService _mailerSendService;

        public RabbitService(IModel channel, MailerSendService mailerSendService)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _mailerSendService = mailerSendService ?? throw new ArgumentNullException(nameof(mailerSendService));
        }

        public void StartConsuming(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var parts = message.Split(',');
                string nome = parts[0].Split(':')[1].Trim();
                string email = parts[1].Split(':')[1].Trim();
                string clientid = parts[2].Split(':')[1].Trim();

                var mailerModel = new MailerModel
                {
                    ClientId = clientid,
                    Nome = nome,
                    Email = email
                };
                
                await _mailerSendService.SendEmailAsync(mailerModel);

            };

            _channel.BasicConsume(queue: "cadastro.em.analise.email",
                                  autoAck: true,
                                  consumer: consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                Task.Delay(1000, stoppingToken).Wait(stoppingToken);
            }
        }
    }
}
