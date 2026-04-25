using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;

namespace MathPillars.Api.Modulos.Probabilidade;

public class GaussianaServico
{
    /// <summary>
    /// Calcula os pontos de uma curva de distribuicao normal (Gaussiana).
    /// </summary>
    public async IAsyncEnumerable<ResultadoSSE<PontoGaussiana[]>> GerarCurvaGaussianaComStreamingSSE(double media, double desvioPadrao, double min, double max, int pontos)
    {
        var pontosLista = new List<PontoGaussiana>();
        var passo = (max - min) / (pontos - 1);

        for (int i = 0; i < pontos; i++)
        {
            var x = min + (i * passo);
            var y = CalcularPDF(x, media, desvioPadrao);
            pontosLista.Add(new PontoGaussiana(x, y));

            if (i % (pontos / 5 + 1) == 0)
            {
                yield return ResultadoSSE<PontoGaussiana[]>.CriarProgressoParcial(i * 100 / pontos, $"Calculando densidade em x={x:F2}...");
                await Task.Delay(50);
            }
        }

        yield return ResultadoSSE<PontoGaussiana[]>.CriarConclusao(pontosLista.ToArray());
    }

    private double CalcularPDF(double x, double media, double desvioPadrao)
    {
        var expoente = -Math.Pow(x - media, 2) / (2 * Math.Pow(desvioPadrao, 2));
        var denominador = desvioPadrao * Math.Sqrt(2 * Math.PI);
        return Math.Exp(expoente) / denominador;
    }
}
