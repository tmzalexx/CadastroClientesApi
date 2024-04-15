using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CadastroClientesApi.api.Models;

public class Password
{
    public string? Passwordhash { get; set; }
}
