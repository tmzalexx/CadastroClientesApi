using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CadastroClientesApi.api.Models;

public class Status
{    
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ClientId { get; set; }
    public bool Enviado { get; set; }
}