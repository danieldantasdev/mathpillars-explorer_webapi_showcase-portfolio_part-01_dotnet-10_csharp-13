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
    private readonly HessianaServico _hessianaServico;

    public CalculoControlador(
        GradienteServico gradienteServico, 
        LossLandscapeServico lossLandscapeServico,
        JacobianaServico jacobianaServico,
        ComparadorOtimizadoresServico comparadorServico,
        HessianaServico hessianaServico)
    {
        _gradienteServico = gradienteServico;
        _lossLandscapeServico = lossLandscapeServico;
        _jacobianaServico = jacobianaServico;
        _comparadorServico = comparadorServico;
        _hessianaServico = hessianaServico;
    }

    [HttpPost("gradiente")]
    public ActionResult<Vetor> PostCalcularGradiente([FromBody] RequisicaoGradiente requisicao)
    {
        var ponto = new Vetor(requisicao.Ponto);
        return Ok(_gradienteServico.CalcularGradiente(GetFuncao(requisicao.FuncaoNome), ponto));
    }

    [HttpPost("hessiana")]
    public ActionResult<Matriz> PostCalcularHessiana([FromBody] RequisicaoHessiana requisicao)
    {
        var ponto = new Vetor(requisicao.Ponto);
        return Ok(_hessianaServico.CalcularHessiana(GetFuncao(requisicao.FuncaoNome), ponto));
    }

    private Func<double[], double> GetFuncao(string nome)
    {
        return nome.ToLower() switch
        {
            "rosenbrock" => p => Math.Pow(1 - p[0], 2) + 100 * Math.Pow(p[1] - p[0] * p[0], 2),
            _ => p => p.Sum(x => x * x)
        };
    }

    [HttpGet("loss-landscape/stream")]
    public async Task GetGerarLossLandscapeComStreaming([FromQuery] RequisicaoLossLandscape req)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        await foreach (var evento in _lossLandscapeServico.GerarSuperficieComStreamingSSE(req.FuncaoNome, req.MinX, req.MaxX, req.MinY, req.MaxY))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }
 
    [HttpGet("otimizador/comparar/stream")]
    public async Task GetCompararOtimizadores([FromQuery] RequisicaoComparacaoOtimizadores req)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        await foreach (var evento in _comparadorServico.CompararCaminhosSSE(req.FuncaoNome, req.XInicial, req.YInicial, req.LearningRate, req.Iteracoes))
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
