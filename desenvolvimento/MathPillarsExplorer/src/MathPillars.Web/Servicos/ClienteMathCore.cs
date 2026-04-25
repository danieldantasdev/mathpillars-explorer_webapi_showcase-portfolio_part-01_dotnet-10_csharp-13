using System.Net.Http.Json;

namespace MathPillars.Web.Servicos;

/// <summary>
/// Cliente principal para comunicacao com a MathPillars.Api.
/// Centraliza todas as chamadas de calculo matematico (REST e SSE).
/// </summary>
public class ClienteMathCore
{
    private readonly HttpClient _httpClient;
    private readonly LeitorSSE _leitorSSE;

    public ClienteMathCore(HttpClient httpClient, LeitorSSE leitorSSE)
    {
        _httpClient = httpClient;
        _leitorSSE = leitorSSE;
    }

    // Algebra Linear - Endpoints Sincronos
    public async Task<TResultado?> PostAsync<TRequisicao, TResultado>(string rota, TRequisicao requisicao)
    {
        var resposta = await _httpClient.PostAsJsonAsync(rota, requisicao);
        resposta.EnsureSuccessStatusCode();
        return await resposta.Content.ReadFromJsonAsync<TResultado>();
    }

    // Streaming SSE
    public IAsyncEnumerable<TResultado> ObterStreamSSE<TResultado>(string rota)
    {
        return _leitorSSE.LerStreamAsync<TResultado>(rota);
    }
}
