using MathPillars.Comum.Contratos; using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;
using Microsoft.AspNetCore.Mvc;

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
    public async IAsyncEnumerable<ResultadoSSE<PontoGaussiana[]>> GetGerarGaussianaComStreaming(
        [FromQuery] double media,
        [FromQuery] double desvioPadrao,
        [FromQuery] double min,
        [FromQuery] double max,
        [FromQuery] int pontos)
    {
        await foreach (var evento in _gaussianaServico.GerarCurvaGaussianaComStreamingSSE(media, desvioPadrao, min, max, pontos))
            yield return evento;
    }
}
