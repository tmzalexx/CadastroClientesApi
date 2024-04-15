using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CadastroClientesApi.api.Models;
public class Address
{
    public string? Street { get; set; }
    public int Number { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZIPCode { get; set; }
}
