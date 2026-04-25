using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MathPillars.Web.Servicos;

/// <summary>
/// Cliente responsavel por realizar a leitura de streams Server-Sent Events (SSE) da API.
/// Permite o processamento reativo de calculos assincronos no Blazor.
/// </summary>
public class LeitorSSE
{
    private readonly HttpClient _httpClient;

    public LeitorSSE(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async IAsyncEnumerable<T> LerStreamAsync<T>(string url, [EnumeratorCancellation] CancellationToken cancelamento = default)
    {
        using var requisicao = new HttpRequestMessage(HttpMethod.Get, url);
        requisicao.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

        using var resposta = await _httpClient.SendAsync(requisicao, HttpCompletionOption.ResponseHeadersRead, cancelamento);
        resposta.EnsureSuccessStatusCode();

        using var fluxo = await resposta.Content.ReadAsStreamAsync(cancelamento);
        using var leitor = new StreamReader(fluxo);

        while (!leitor.EndOfStream && !cancelamento.IsCancellationRequested)
        {
            var linha = await leitor.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(linha)) continue;

            if (linha.StartsWith("data:"))
            {
                var json = linha.Substring(5).Trim();
                var dado = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (dado != null) yield return dado;
            }
        }
    }
}
