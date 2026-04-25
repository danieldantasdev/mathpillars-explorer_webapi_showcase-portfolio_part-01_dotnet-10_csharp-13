using MathPillars.Comum.Contratos; 
using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.AlgebraLinear;
using Microsoft.AspNetCore.Mvc;
using MathPillars.Api.Compartilhado;

namespace MathPillars.Api.Modulos.AlgebraLinear;

[ApiController]
[Route("api/algebra-linear")]
public class AlgebraLinearControlador : ControllerBase
{
    private readonly ProdutoEscalarServico _produtoEscalarServico;
    private readonly SimilaridadeCossenoServico _similaridadeCossenoServico;
    private readonly SVDServico _svdServico;
    private readonly PCAServico _pcaServico;
    private readonly AutovetoresServico _autovetoresServico;

    public AlgebraLinearControlador(
        ProdutoEscalarServico produtoEscalarServico,
        SimilaridadeCossenoServico similaridadeCossenoServico,
        SVDServico svdServico,
        PCAServico pcaServico,
        AutovetoresServico autovetoresServico)
    {
        _produtoEscalarServico = produtoEscalarServico;
        _similaridadeCossenoServico = similaridadeCossenoServico;
        _svdServico = svdServico;
        _pcaServico = pcaServico;
        _autovetoresServico = autovetoresServico;
    }

    [HttpPost("produto-escalar")]
    public ActionResult<Escalar> PostCalcularProdutoEscalar([FromBody] RequisicaoProdutoEscalar requisicao)
    {
        var vetorA = new Vetor(requisicao.VetorA);
        var vetorB = new Vetor(requisicao.VetorB);
        return Ok(_produtoEscalarServico.CalcularProdutoEscalar(vetorA, vetorB));
    }

    [HttpPost("similaridade-cosseno")]
    public ActionResult<Escalar> PostCalcularSimilaridadeCosseno([FromBody] RequisicaoSimilaridade requisicao)
    {
        var vetorA = new Vetor(requisicao.VetorA);
        var vetorB = new Vetor(requisicao.VetorB);
        return Ok(_similaridadeCossenoServico.CalcularSimilaridade(vetorA, vetorB));
    }

    [HttpGet("svd/stream")]
    public async Task GetDecomporSVDComStreaming(
        [FromQuery] int linhas,
        [FromQuery] int colunas,
        [FromQuery] string elementosJson)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        var elementos = System.Text.Json.JsonSerializer.Deserialize<double[]>(elementosJson)!;
        var elementosMatrix = new double[linhas][];
        for (var i = 0; i < linhas; i++)
        {
            elementosMatrix[i] = new double[colunas];
            for (var j = 0; j < colunas; j++)
            {
                elementosMatrix[i][j] = elementos[i * colunas + j];
            }
        }

        var matriz = new Matriz(elementosMatrix);

        await foreach (var evento in _svdServico.DecomporComStreamingSSE(matriz))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }

    [HttpPost("autovetores")]
    public ActionResult<ResultadoAutovetores> PostCalcularAutovetores([FromBody] RequisicaoAutovetores requisicao)
    {
        var matriz = new Matriz(requisicao.Elementos);
        return Ok(_autovetoresServico.CalcularAutovetoresEAutovalores(matriz));
    }
}
