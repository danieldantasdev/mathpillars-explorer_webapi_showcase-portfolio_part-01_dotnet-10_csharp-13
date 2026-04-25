using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace MathPillars.Api.Compartilhado;

/// <summary>
/// Auxiliar para formatacao e envio de Server-Sent Events (SSE).
/// </summary>
public static class SSEHelper
{
    private static readonly JsonSerializerOptions OpcoesJson = new() 
    { 
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task EscreverEventoAsync<T>(HttpResponse resposta, T dado)
    {
        var json = JsonSerializer.Serialize(dado, OpcoesJson);
        await resposta.WriteAsync($"data: {json}\n\n");
        await resposta.Body.FlushAsync();
    }
}
