using MathPillars.Comum.Contratos; using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.CalculoMultivariavel;
using Microsoft.AspNetCore.Mvc;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;


/// <summary>
/// Controlador responsavel por receber e rotear requisicoes do modulo de Calculo Multivariavel.
/// </summary>
[ApiController]
[Route("api/calculo")]
public class CalculoControlador : ControllerBase
{
    private readonly GradienteServico _gradienteServico;
    private readonly LossLandscapeServico _lossLandscapeServico;

    public CalculoControlador(GradienteServico gradienteServico, LossLandscapeServico lossLandscapeServico)
    {
        _gradienteServico = gradienteServico;
        _lossLandscapeServico = lossLandscapeServico;
    }

    [HttpPost("gradiente")]
    public ActionResult<Vetor> PostCalcularGradiente([FromBody] RequisicaoGradiente requisicao)
    {
        var ponto = new Vetor(requisicao.Ponto);
        return Ok(_gradienteServico.CalcularGradiente(p => p.Sum(x => x * x), ponto));
    }

    [HttpGet("loss-landscape/stream")]
    public async IAsyncEnumerable<ResultadoSSE<PontoSuperficie3D[]>> GetGerarLossLandscapeComStreaming(
        [FromQuery] string funcaoNome,
        [FromQuery] double minX,
        [FromQuery] double maxX,
        [FromQuery] double minY,
        [FromQuery] double maxY)
    {
        await foreach (var evento in _lossLandscapeServico.GerarSuperficieComStreamingSSE(funcaoNome, minX, maxX, minY, maxY))
            yield return evento;
    }
}
