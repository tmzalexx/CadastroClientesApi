using CadastroClientesApi.api.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using CadastroClientesApi.api.Infrastructure;
using MongoDB.Bson;
using CadastroClientesApi.api.Models;

namespace CadastroClientesApi.api.Controllers;

[ApiController]
[Route("api/registration/password")]
public class PasswordController : ControllerBase
{
    private readonly MongoDb _mongoDbService;

    public PasswordController(MongoDb mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPassword([FromBody] KeyRequest keyRequest, [FromHeader(Name = "ClientId")] ObjectId id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(keyRequest.Key))
        {
            return BadRequest("The key cannot be null or empty.");
        }

    string hashedPassword = GetHashedPassword(keyRequest.Key);

        var password = new Password
        {
            Passwordhash = hashedPassword
        };

        try
        {
            var basicData = await _mongoDbService.GetDocumentId<BasicData>("basicDataCollection", id);
            if (basicData == null)
            {
                return BadRequest("Client not found.");
            }

            basicData.Password = password;

            await _mongoDbService.ReplaceDocument<BasicData>("basicDataCollection", id, basicData);
            return Ok("Password received and stored successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while saving the password: {ex.Message}");
        }
    }

    private string GetHashedPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}