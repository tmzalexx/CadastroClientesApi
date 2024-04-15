using CadastroClienteApi.mailer.Infrastructure;
using CadastroClienteApi.mailer.Mailer;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<RegistrationRepository>();

var host = builder.Build();
host.Run();
