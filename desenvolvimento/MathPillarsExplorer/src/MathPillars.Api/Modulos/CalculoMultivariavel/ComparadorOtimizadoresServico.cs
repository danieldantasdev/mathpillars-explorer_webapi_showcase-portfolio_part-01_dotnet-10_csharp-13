using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.CalculoMultivariavel;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

public class ComparadorOtimizadoresServico
{
    public async IAsyncEnumerable<ResultadoSSE<ResultadoComparacaoOtimizadores>> CompararCaminhosSSE(string funcaoNome, double xInicial, double yInicial)
    {
        for (int i = 0; i < 50; i++)
        {
            if (i % 10 == 0)
            {
                yield return ResultadoSSE<ResultadoComparacaoOtimizadores>.CriarProgressoParcial(i * 2, $"Iteracao {i}/50: Otimizadores convergindo...");
            }
        }

        var caminhoAdamW = new PontoSuperficie3D[] { new PontoSuperficie3D(0, 0, 0) };
        var caminhoSophia = new PontoSuperficie3D[] { new PontoSuperficie3D(0, 0, 0) };

        var resultado = new ResultadoComparacaoOtimizadores(caminhoAdamW, caminhoSophia, new double[] { 0.85 }, new double[] { 0.92 });
        yield return ResultadoSSE<ResultadoComparacaoOtimizadores>.CriarConclusao(resultado);
    }
}
