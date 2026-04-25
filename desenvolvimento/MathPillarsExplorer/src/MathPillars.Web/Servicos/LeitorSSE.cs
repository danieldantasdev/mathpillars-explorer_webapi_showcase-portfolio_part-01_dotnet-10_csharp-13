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
        HttpRequestMessage? requisicao = null;
        try
        {
            requisicao = new HttpRequestMessage(HttpMethod.Get, url);
            requisicao.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

            using var resposta = await _httpClient.SendAsync(requisicao, HttpCompletionOption.ResponseHeadersRead, cancelamento);
            resposta.EnsureSuccessStatusCode();

            using var fluxo = await resposta.Content.ReadAsStreamAsync(cancelamento);
            using var leitor = new StreamReader(fluxo);

            while (!cancelamento.IsCancellationRequested)
            {
                string? linha;
                try
                {
                    linha = await leitor.ReadLineAsync();
                }
                catch
                {
                    break; // Erro de leitura ou conexao fechada
                }

                if (linha == null) break; // Fim do stream
                
                if (string.IsNullOrWhiteSpace(linha)) continue;

                if (linha.StartsWith("data:"))
                {
                    var json = linha.Substring(5).Trim();
                    T? dado = default;
                    try
                    {
                        dado = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch
                    {
                        continue; // Ignora pacotes JSON invalidos
                    }
                    
                    if (dado != null) yield return dado;
                }
            }
        }
        finally
        {
            requisicao?.Dispose();
        }
    }
}
