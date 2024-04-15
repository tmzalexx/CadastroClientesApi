using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CadastroClienteApi.mailer.Models
{
    public class BasicDataModel
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

        public bool SendEmail { get; set; } 

        public Address? Address { get; set; }

        public Finance? Finance { get; set; }

        public Password? Password { get; set; }
    }

    public class Address
    {
 public string? Street { get; set; }
    public int Number { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZIPCode { get; set; }
    }

    public class Finance
    {
        public string? Renda { get; set; }

        public string? Patrimonio { get; set; }
    }

    public class Password
    {
        public string? Passwordhash { get; set; }
    }
}
