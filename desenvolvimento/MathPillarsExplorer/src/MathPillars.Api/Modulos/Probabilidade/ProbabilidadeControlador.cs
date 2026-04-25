using MathPillars.Comum.Contratos; 
using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;
using Microsoft.AspNetCore.Mvc;
using MathPillars.Api.Compartilhado;

namespace MathPillars.Api.Modulos.Probabilidade;

[ApiController]
[Route("api/probabilidade")]
public class ProbabilidadeControlador : ControllerBase
{
    private readonly BayesServico _bayesServico;
    private readonly GaussianaServico _gaussianaServico;
    private readonly EntropiaCruzadaServico _entropiaServico;
    private readonly MarkovServico _markovServico;

    public ProbabilidadeControlador(
        BayesServico bayesServico, 
        GaussianaServico gaussianaServico,
        EntropiaCruzadaServico entropiaServico,
        MarkovServico markovServico)
    {
        _bayesServico = bayesServico;
        _gaussianaServico = gaussianaServico;
        _entropiaServico = entropiaServico;
        _markovServico = markovServico;
    }

    [HttpPost("bayes")]
    public ActionResult<ResultadoBayes> PostCalcularBayes([FromBody] RequisicaoBayes requisicao)
    {
        return Ok(_bayesServico.CalcularPosterior(requisicao.PriorH, requisicao.VerossimilhancaEH, requisicao.VerossimilhancaENaoH));
    }

    [HttpGet("gaussiana/stream")]
    public async Task GetGerarGaussianaComStreaming(
        [FromQuery] double media,
        [FromQuery] double desvioPadrao,
        [FromQuery] double min,
        [FromQuery] double max,
        [FromQuery] int pontos)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        await foreach (var evento in _gaussianaServico.GerarCurvaGaussianaComStreamingSSE(media, desvioPadrao, min, max, pontos))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }

    [HttpPost("entropia-cruzada")]
    public ActionResult<ResultadoEntropiaCruzada> PostCalcularEntropia([FromBody] dynamic req)
    {
        double[] real = new[] { 1.0, 0.0, 0.0 };
        double[] pred = new[] { 0.7, 0.2, 0.1 };
        return Ok(_entropiaServico.Calcular(real, pred));
    }

    [HttpPost("markov")]
    public ActionResult<ResultadoMarkov> PostSimularMarkov([FromBody] dynamic req)
    {
        // Exemplo: 2 estados (S0: Sol, S1: Chuva)
        var matriz = new double[][] {
            new double[] { 0.7, 0.3 },
            new double[] { 0.4, 0.6 }
        };
        return Ok(_markovServico.Simular(matriz, 100, 0));
    }
}
