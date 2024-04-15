using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CadastroClientesApi.api.Models;

public class BasicData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime? Birthdate { get; set; }

    [Required]
    public string? Cpf { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Phone { get; set; }
    public bool SendEmail { get; set; } = false;
    public Address? Address { get; set; }
    public Finance? Finance { get; set; }
    public Password? Password { get; set; }

}