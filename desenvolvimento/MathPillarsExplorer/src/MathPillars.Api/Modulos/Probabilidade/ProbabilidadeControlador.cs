using MathPillars.Comum.Contratos; 
using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;
using Microsoft.AspNetCore.Mvc;
using MathPillars.Api.Compartilhado;

namespace MathPillars.Api.Modulos.Probabilidade;

/// <summary>
/// Controlador responsavel por receber e rotear requisicoes do modulo de Probabilidade e Estatística.
/// </summary>
[ApiController]
[Route("api/probabilidade")]
public class ProbabilidadeControlador : ControllerBase
{
    private readonly BayesServico _bayesServico;
    private readonly GaussianaServico _gaussianaServico;

    public ProbabilidadeControlador(BayesServico bayesServico, GaussianaServico gaussianaServico)
    {
        _bayesServico = bayesServico;
        _gaussianaServico = gaussianaServico;
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
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        await foreach (var evento in _gaussianaServico.GerarCurvaGaussianaComStreamingSSE(media, desvioPadrao, min, max, pontos))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }
}
