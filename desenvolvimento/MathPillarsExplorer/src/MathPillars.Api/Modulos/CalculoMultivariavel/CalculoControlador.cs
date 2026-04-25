using MathPillars.Comum.Contratos; 
using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.CalculoMultivariavel;
using Microsoft.AspNetCore.Mvc;
using MathPillars.Api.Compartilhado;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

[ApiController]
[Route("api/calculo")]
public class CalculoControlador : ControllerBase
{
    private readonly GradienteServico _gradienteServico;
    private readonly LossLandscapeServico _lossLandscapeServico;
    private readonly JacobianaServico _jacobianaServico;
    private readonly ComparadorOtimizadoresServico _comparadorServico;

    public CalculoControlador(
        GradienteServico gradienteServico, 
        LossLandscapeServico lossLandscapeServico,
        JacobianaServico jacobianaServico,
        ComparadorOtimizadoresServico comparadorServico)
    {
        _gradienteServico = gradienteServico;
        _lossLandscapeServico = lossLandscapeServico;
        _jacobianaServico = jacobianaServico;
        _comparadorServico = comparadorServico;
    }

    [HttpPost("gradiente")]
    public ActionResult<Vetor> PostCalcularGradiente([FromBody] RequisicaoGradiente requisicao)
    {
        var ponto = new Vetor(requisicao.Ponto);
        return Ok(_gradienteServico.CalcularGradiente(p => p.Sum(x => x * x), ponto));
    }

    [HttpGet("loss-landscape/stream")]
    public async Task GetGerarLossLandscapeComStreaming(
        [FromQuery] string funcaoNome,
        [FromQuery] double minX,
        [FromQuery] double maxX,
        [FromQuery] double minY,
        [FromQuery] double maxY)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        await foreach (var evento in _lossLandscapeServico.GerarSuperficieComStreamingSSE(funcaoNome, minX, maxX, minY, maxY))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }

    [HttpGet("otimizador/comparar/stream")]
    public async Task GetCompararOtimizadores(
        [FromQuery] string funcaoNome,
        [FromQuery] double xInicial,
        [FromQuery] double yInicial,
        [FromQuery] double learningRate = 0.01,
        [FromQuery] int iteracoes = 100)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        await foreach (var evento in _comparadorServico.CompararCaminhosSSE(funcaoNome, xInicial, yInicial, learningRate, iteracoes))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }

    [HttpPost("jacobiana")]
    public ActionResult<Matriz> PostCalcularJacobiana([FromBody] RequisicaoJacobiana requisicao)
    {
        var ponto = new Vetor(requisicao.Ponto);
        return Ok(_jacobianaServico.CalcularJacobiana(p => new double[] { p[0] * p[0] + p[1], p[1] * p[1] + p[0] }, ponto));
    }
}
