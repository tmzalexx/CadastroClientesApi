using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CadastroClientesApi.api.Models;

public class Finance
{
    public decimal Renda { get; set; }
    public decimal Patrimonio { get; set; }
}