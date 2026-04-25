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
    public async Task GetDecomporSVDComStreaming([FromQuery] RequisicaoSVD req)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        var elementos = System.Text.Json.JsonSerializer.Deserialize<double[]>(req.ElementosJson)!;
        var elementosMatrix = new double[req.Linhas][];
        for (var i = 0; i < req.Linhas; i++)
        {
            elementosMatrix[i] = new double[req.Colunas];
            for (var j = 0; j < req.Colunas; j++)
            {
                elementosMatrix[i][j] = elementos[i * req.Colunas + j];
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

    [HttpGet("pca/stream")]
    public async Task GetCalcularPCA([FromQuery] RequisicaoPCA req)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        var elementos = System.Text.Json.JsonSerializer.Deserialize<double[]>(req.ElementosJson)!;
        var dados = new double[req.Linhas][];
        for (var i = 0; i < req.Linhas; i++)
        {
            dados[i] = new double[req.Colunas];
            for (var j = 0; j < req.Colunas; j++)
                dados[i][j] = elementos[i * req.Colunas + j];
        }

        await foreach (var evento in _pcaServico.CalcularPCAComStreamingSSE(dados, req.Componentes))
        {
            await SSEHelper.EscreverEventoAsync(Response, evento);
        }
    }
}

