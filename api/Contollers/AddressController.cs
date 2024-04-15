using Microsoft.AspNetCore.Mvc;
using CadastroClientesApi.api.Models;
using CadastroClientesApi.api.Infrastructure;
using MongoDB.Bson;

namespace CadastroClientesApi.api.Controllers;

[ApiController]
[Route("api/registration/address")]
public class AddressController : ControllerBase
{
private readonly MongoDb _mongoDbService;

public AddressController(MongoDb mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody] Address address, [FromHeader(Name = "ClientId")] ObjectId id)
    {
        var basicData = await _mongoDbService.GetDocumentId<BasicData>("basicDataCollection", id);

        if (basicData == null)
        {
            return BadRequest("Client not found.");
        }

        basicData.Address = address;

        await _mongoDbService.ReplaceDocument<BasicData>("basicDataCollection", id, basicData);

        return Ok(id.ToString());
    }
}

