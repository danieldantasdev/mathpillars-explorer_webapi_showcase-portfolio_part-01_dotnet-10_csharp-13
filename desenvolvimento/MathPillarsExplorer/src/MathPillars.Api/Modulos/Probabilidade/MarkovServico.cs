using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;

namespace MathPillars.Api.Modulos.Probabilidade;

public class MarkovServico
{
    public ResultadoMarkov Simular(double[][] matrizTransicao, int passos, int estadoInicial)
    {
        var n = matrizTransicao.Length;
        var estadoAtual = estadoInicial;
        var historico = new List<int> { estadoAtual };
        var random = new Random();

        for (int i = 0; i < passos; i++)
        {
            var r = random.NextDouble();
            var acumulado = 0.0;
            var novoEstado = n - 1;
            for (int j = 0; j < n; j++)
            {
                acumulado += matrizTransicao[estadoAtual][j];
                if (r <= acumulado)
                {
                    novoEstado = j;
                    break;
                }
            }
            estadoAtual = novoEstado;
            historico.Add(estadoAtual);
        }

        // Calcula distribuicao estacionaria aproximada
        var contagens = new int[n];
        foreach (var e in historico) contagens[e]++;
        var distribuicao = contagens.Select(c => (double)c / historico.Count).ToArray();

        return new ResultadoMarkov(historico.ToArray(), distribuicao);
    }
}

