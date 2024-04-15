using MongoDB.Bson;
using MongoDB.Driver;
using CadastroClientesApi.api.Models;

namespace CadastroClientesApi.api.Infrastructure;

public class MongoDb
{
    private readonly IMongoDatabase _database;
    private readonly string _connectionString;
    private readonly string _databaseName;

    public MongoDb(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("MongoDBSettings:ConnectionString")?.Value ?? throw new ArgumentNullException(nameof(_connectionString));
        _databaseName = configuration.GetSection("MongoDBSettings:DatabaseName")?.Value ?? throw new ArgumentNullException(nameof(_databaseName));
        var client = new MongoClient(_connectionString);
        _database = client.GetDatabase(_databaseName);
    }

    public async Task SendDocument<T>(string nomeColecao, T documento)
    {
        var colecao = _database.GetCollection<T>(nomeColecao);
        await colecao.InsertOneAsync(documento);
    }

    public async Task<bool> DocumentExists<T>(string nomeColecao, ObjectId clientId)
    {
        var colecao = _database.GetCollection<T>(nomeColecao);
        var filtro = Builders<T>.Filter.Eq("ClientId", clientId);
        var documento = await colecao.Find(filtro).FirstOrDefaultAsync();
        return documento != null;
    }

    public async Task<T> GetDocumentId<T>(string nomeColecao, ObjectId id)
    {
        var colecao = _database.GetCollection<T>(nomeColecao);
        var filtro = Builders<T>.Filter.Eq("_id", id);
        return await colecao.Find(filtro).FirstOrDefaultAsync();
    }

        public async Task<T> GetDocumentClientId<T>(string nomeColecao, ObjectId id)
    {
        var colecao = _database.GetCollection<T>(nomeColecao);
        var filtro = Builders<T>.Filter.Eq("ClientId", id);
        return await colecao.Find(filtro).FirstOrDefaultAsync();
    }
       public async Task ReplaceDocument<T>(string nomeColecao, ObjectId id, T documento)
    {
        var colecao = _database.GetCollection<T>(nomeColecao);
        var filtro = Builders<T>.Filter.Eq("_id", id);
        await colecao.ReplaceOneAsync(filtro, documento);
    }

    public async Task<T> GetDocumentCpf<T>(string nomeColecao, string cpf)
    {
        if (cpf == null)
            throw new ArgumentNullException(nameof(cpf));

        var colecao = _database.GetCollection<T>(nomeColecao);
        var filtro = Builders<T>.Filter.Eq("Cpf", cpf);
        return await colecao.Find(filtro).FirstOrDefaultAsync();
    }

  internal async Task InsertOneAsync(string v,BasicData basicData)
  {
    throw new NotImplementedException();
  }
}

