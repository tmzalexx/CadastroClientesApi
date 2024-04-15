using System;
using MongoDB.Bson;
using Mailjet.Client;
using System.Threading.Tasks;
using Mailjet.Client.Resources;
using CadastroClienteApi.mailer.Models;
using Microsoft.Extensions.Configuration;
using CadastroClienteApi.mailer.Templates;
using CadastroClienteApi.mailer.Infrastructure;

namespace CadastroClienteApi.mailer.Services;
    public class MailerSendService
    {
        private readonly RegistrationRepository _registrationRepository;
        private readonly IMailjetClient _client;
        private readonly string _apiKey;
        private readonly string _secretKey;

        public MailerSendService(IConfiguration configuration, RegistrationRepository registrationRepository)
        {
            _apiKey = configuration.GetConnectionString("MailjetApiKey") ?? string.Empty;
            _secretKey = configuration.GetConnectionString("MailjetSecretKey") ?? string.Empty;
            _client = new MailjetClient(_apiKey, _secretKey);
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));
        }

        public async Task<bool> SendEmailAsync(MailerModel mailerModel)
        {
            string templateHtml = EmailTemplates.CadastroEmAnaliseHtml;
            string emailHtml = templateHtml.Replace("{{nome}}", mailerModel.Nome);
            string templateTxt = EmailTemplates.CadastroEmAnaliseTxt;
            string emailTxt = templateTxt.Replace("{{nome}}", mailerModel.Nome);

            var basicData = await _registrationRepository.GetDocumentById<BasicDataModel>("basicDataCollection", ObjectId.Parse(mailerModel.ClientId));
            if (basicData == null)
            {
                throw new Exception("Client not found.");
            }

            if (!basicData.SendEmail)
            {
                Console.WriteLine("Sending email...");
                try
                {
                    MailjetRequest request = new MailjetRequest
                    {
                        Resource = Send.Resource
                    }
                    .Property(Send.FromEmail, "thomazalex@gmail.com")
                    .Property(Send.FromName, "PATH BANK")
                    .Property(Send.Subject, "PATH BANK - Cadastro em análise")
                    .Property(Send.HtmlPart, emailHtml)
                    .Property(Send.TextPart, emailTxt)
                    .Property(Send.To, mailerModel.Email);

                    MailjetResponse response = await _client.PostAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        await _registrationRepository.UpdateSendEmailFlag<BasicDataModel>("basicDataCollection", ObjectId.Parse(mailerModel.ClientId));
                        Console.WriteLine( "Email sent successfully!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Error sending email. Status code: {response.StatusCode}. Error: {response.GetErrorMessage()}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email. Error: {ex.Message}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Email has already been sent to this client.");
                return false;
            }

        }
    }

