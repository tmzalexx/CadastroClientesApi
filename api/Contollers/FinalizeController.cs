using CadastroClientesApi.api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using CadastroClientesApi.api.Infrastructure;
using System.Text;
using RabbitMQ.Client;

namespace CadastroClientesApi.api.Controllers
{
    [ApiController]
    [Route("api/registration/finalize")]
    public class FinalizaController : ControllerBase
    {
        private readonly MongoDb _mongoDbService;
        private readonly IModel _rabbitMqModel;

        public FinalizaController(MongoDb mongoDbService, IModel rabbitMqModel)
        {
            _mongoDbService = mongoDbService;
            _rabbitMqModel = rabbitMqModel;
        }

        [HttpPost("{clientId}")]
        public async Task<IActionResult> RegisterClient(ObjectId clientId)
        {
            if (clientId == null)
            {
                return BadRequest("ClientId not provided.");
            }

            return await SendToEmailQueue(clientId);
        }

        private async Task<IActionResult> SendToEmailQueue(ObjectId clientId)
        {
            var basicData = await _mongoDbService.GetDocumentId<BasicData>("basicDataCollection", clientId);

            if (basicData == null)
            {
                return NotFound("Client not found in the database. Unable to send to the email queue.");
            }

            if (basicData.Address == null || basicData.Finance == null || basicData.Password == null)
            {
                return BadRequest("Incomplete client registration. Unable to send to the email queue.");
            }
            if (basicData.SendEmail)
            {
                return BadRequest("Client already registered, please wait for analysis.");
            }

            var name = basicData.Name;
            var email = basicData.Email;

            var properties = _rabbitMqModel.CreateBasicProperties();
            properties.Persistent = true;

            var body = Encoding.UTF8.GetBytes($"Nome: {name}, Email: {email}, ClientId: {clientId}");

            try
            {
                _rabbitMqModel.BasicPublish(exchange: "",
                                            routingKey: "cadastro.em.analise.email",
                                            basicProperties: properties,
                                            body: body);
                Console.WriteLine("Message sent to the RabbitMQ queue.");
                return Ok("Data sent to email queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while sending message to the RabbitMQ queue: {ex.Message}");
                return StatusCode(500, "Internal server error while sending message to the RabbitMQ queue.");
            }
        }
    }
}
