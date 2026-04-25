using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.CalculoMultivariavel;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

public class LossLandscapeServico
{
    public async IAsyncEnumerable<ResultadoSSE<PontoSuperficie3D[]>> GerarSuperficieComStreamingSSE(
        string funcaoNome, double minX, double maxX, double minY, double maxY)
    {
        var pontos = new List<PontoSuperficie3D>();
        int resolucao = 20;
        double passoX = (maxX - minX) / resolucao;
        double passoY = (maxY - minY) / resolucao;

        for (int i = 0; i <= resolucao; i++)
        {
            double x = minX + i * passoX;
            for (int j = 0; j <= resolucao; j++)
            {
                double y = minY + j * passoY;
                double z = CalcularFuncao(funcaoNome, x, y);
                pontos.Add(new PontoSuperficie3D(x, y, z));
            }

            if (i % 4 == 0)
            {
                yield return ResultadoSSE<PontoSuperficie3D[]>.CriarProgressoParcial(i * 100 / resolucao, $"Calculando malha 3D: {i}/{resolucao} linhas.");
            }
        }

        yield return ResultadoSSE<PontoSuperficie3D[]>.CriarConclusao(pontos.ToArray());
    }

    private double CalcularFuncao(string nome, double x, double y)
    {
        return nome switch
        {
            "Rosenbrock" => Math.Pow(1 - x, 2) + 100 * Math.Pow(y - x * x, 2),
            _ => x * x + y * y
        };
    }
}
