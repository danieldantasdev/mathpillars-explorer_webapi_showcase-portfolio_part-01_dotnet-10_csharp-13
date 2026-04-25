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
    public async Task GetGerarGaussianaComStreaming([FromQuery] RequisicaoGaussiana req)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        await foreach (var evento in _gaussianaServico.GerarCurvaGaussianaComStreamingSSE(req.Media, req.DesvioPadrao, req.Min, req.Max, req.Pontos))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }

    [HttpPost("entropia-cruzada")]
    public ActionResult<ResultadoEntropiaCruzada> PostCalcularEntropia([FromBody] RequisicaoEntropiaCruzada req)
    {
        return Ok(_entropiaServico.Calcular(req.Real, req.Predito));
    }

    [HttpPost("markov")]
    public ActionResult<ResultadoMarkov> PostSimularMarkov([FromBody] RequisicaoMarkov req)
    {
        return Ok(_markovServico.Simular(req.MatrizTransicao, req.Passos, req.EstadoInicial));
    }
}
