using CadastroClientesApi.api.Models;
using Microsoft.AspNetCore.Mvc;
using CadastroClientesApi.api.Infrastructure;
using MongoDB.Bson;

namespace CadastroClientesApi.api.Controllers;

[ApiController]
[Route("api/registration/finance")]
public class FinanceController : ControllerBase
{
    private readonly MongoDb _mongoDbService;

    public FinanceController(MongoDb mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddFinanceAsync([FromBody] Finance finance, [FromHeader(Name = "ClientId")] ObjectId id)
    {
        if (finance.Renda + finance.Patrimonio < 1000.00m)
        {
            return BadRequest("The sum of Income and Assets must be greater than 1000.00.");
        }

        var basicData = await _mongoDbService.GetDocumentId<BasicData>("basicDataCollection", id);

        if (basicData == null)
        {
            return BadRequest("Client not found");
        }

        basicData.Finance = finance;

        await _mongoDbService.ReplaceDocument<BasicData>("basicDataCollection", id, basicData);

        return Ok(id.ToString());
    }
}