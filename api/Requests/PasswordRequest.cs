using System.ComponentModel.DataAnnotations;

namespace CadastroClientesApi.api.Requests;

public class KeyRequest
{
    [Required]
    public string? Key { get; set; }
}
