using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;

namespace MathPillars.Api.Modulos.Probabilidade;

public class GaussianaServico
{
    public async IAsyncEnumerable<ResultadoSSE<PontoGaussiana[]>> GerarCurvaGaussianaComStreamingSSE(double media, double desvioPadrao, double min, double max, int pontos)
    {
        var resultado = new List<PontoGaussiana>();
        for (int i = 0; i < pontos; i++)
        {
            if (i % 10 == 0)
            {
                yield return ResultadoSSE<PontoGaussiana[]>.CriarProgressoParcial(i * 100 / pontos, "Gerando curva...");
            }
        }

        yield return ResultadoSSE<PontoGaussiana[]>.CriarConclusao(resultado.ToArray());
    }
}
