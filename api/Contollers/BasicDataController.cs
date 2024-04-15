using CadastroClientesApi.api.Models;
using Microsoft.AspNetCore.Mvc;
using CadastroClientesApi.api.Helpers;
using CadastroClientesApi.api.Infrastructure;

namespace CadastroClientesApi.api.Controllers;

[ApiController]
[Route("api/registration/basicdata")]
public class BasicDataController : ControllerBase
{
    private readonly MongoDb _mongoDbService;

    public BasicDataController(MongoDb mongoDbService)
    {
         _mongoDbService = mongoDbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBasicData([FromBody] BasicData basicData)
    {
        if (!CpfValidator.IsValidCpf(basicData.Cpf))
        {
            return BadRequest("Invalid CPF.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
 
        var existingData = await _mongoDbService.GetDocumentCpf<BasicData>("basicDataCollection", basicData.Cpf);
        if (existingData != null)
        {
            return BadRequest("CPF already registered.");
        }

        int age = AgeValidator.CalculateAge(basicData.Birthdate);
        if (age < 18)
        {
            return BadRequest("The client must be over 18 years old.");
        }

        await _mongoDbService.SendDocument<BasicData>("basicDataCollection", basicData);


        return Ok(basicData.Id); 
    }
}

