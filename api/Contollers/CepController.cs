using Microsoft.AspNetCore.Mvc;

namespace CadastroClientesApi.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CepController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public CepController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{zipcode}")]
    public async Task<IActionResult> GetAddressByZipcode(string zipcode)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://www.ceprapido.com/api/addresses/{zipcode}?countryCode=BRA");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Erro ao obter informações do CEP");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}
