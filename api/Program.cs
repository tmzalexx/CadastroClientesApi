using CadastroClientesApi.api.Infrastructure;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<MongoDb>();

var rabbitMQConnectionStrings = builder.Configuration.GetSection("RabbitMQConnectionStrings").Get<List<string>>() ?? new List<string>();
builder.Services.AddSingleton(rabbitMQConnectionStrings);
builder.Services.AddSingleton<RabbitMQConnectionFactory>();

// Modifique a configuração do serviço IModel para usar a RabbitMQConnectionFactory
builder.Services.AddSingleton<IModel>(provider =>
{
    var factory = provider.GetRequiredService<RabbitMQConnectionFactory>();
    var connection = factory.CreateConnection(0); // Supondo que você quer usar a primeira string de conexão
    return connection.CreateModel();
});
var app = builder.Build();

// Configurar o middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder => 
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);
app.MapControllers();

app.Run();