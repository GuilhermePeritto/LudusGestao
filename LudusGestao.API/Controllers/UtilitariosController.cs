using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Application.DTOs.Utilitarios;
using LudusGestao.Application.Common.Models;
using LudusGestao.Domain.Interfaces.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace LudusGestao.API.Controllers;

[ApiController]
[Route("api/utilitarios")]
public class UtilitariosController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISeedService _seedService;

    public UtilitariosController(IHttpClientFactory httpClientFactory, ISeedService seedService)
    {
        _httpClientFactory = httpClientFactory;
        _seedService = seedService;
    }

    [HttpGet("cep/{cep}")]
    public async Task<IActionResult> BuscarEnderecoPorCep(string cep)
    {
        try
        {
            // Remove caracteres não numéricos do CEP
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            
            if (cepLimpo.Length != 8)
                return BadRequest(new ApiResponse<object>(default) { Success = false, Message = "CEP deve conter 8 dígitos" });

            // Busca na API ViaCEP
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");
            
            if (!response.IsSuccessStatusCode)
                return NotFound(new ApiResponse<object>(default) { Success = false, Message = "CEP não encontrado" });

            var jsonString = await response.Content.ReadAsStringAsync();
            var viaCepResponse = JsonSerializer.Deserialize<ViaCepResponse>(jsonString);

            if (viaCepResponse?.Erro == true)
                return NotFound(new ApiResponse<object>(default) { Success = false, Message = "CEP não encontrado" });

            var endereco = new EnderecoCepDTO
            {
                Cep = viaCepResponse.Cep,
                Rua = viaCepResponse.Logradouro,
                Bairro = viaCepResponse.Bairro,
                Cidade = viaCepResponse.Localidade,
                Estado = viaCepResponse.Uf,
                Numero = ""
            };

            return Ok(new ApiResponse<EnderecoCepDTO>(endereco, "Endereço encontrado com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(default) { Success = false, Message = "Erro ao buscar endereço" });
        }
    }

    [HttpPost("seed")]
    public async Task<IActionResult> ExecutarSeed()
    {
        try
        {
            var resultado = await _seedService.SeedDadosBaseAsync();
            
            if (resultado)
            {
                return Ok(new { message = "Dados base inseridos com sucesso!" });
            }
            else
            {
                return BadRequest(new { message = "Dados já existem ou erro ao inserir dados base." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Erro ao executar seed: {ex.Message}" });
        }
    }
}

// Classe para deserializar a resposta da API ViaCEP
public class ViaCepResponse
{
    public string Cep { get; set; }
    public string Logradouro { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Localidade { get; set; }
    public string Uf { get; set; }
    public string Ibge { get; set; }
    public string Gia { get; set; }
    public string Ddd { get; set; }
    public string Siafi { get; set; }
    public bool? Erro { get; set; }
} 