using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.AlgebraLinear;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace MathPillars.Api.Modulos.AlgebraLinear;

public class PCAServico
{
    /// <summary>
    /// Executa a Analise de Componentes Principais com streaming de progresso.
    /// </summary>
    public async IAsyncEnumerable<ResultadoSSE<ResultadoPCA>> CalcularPCAComStreamingSSE(double[][] dados, int componentesAlvo)
    {
        var matrizOriginal = Matrix<double>.Build.DenseOfRowArrays(dados);
        var m = matrizOriginal.RowCount;
        var n = matrizOriginal.ColumnCount;

        yield return ResultadoSSE<ResultadoPCA>.CriarProgressoParcial(10, "Centralizando dados (subtraindo a media)...");
        var matrizCentralizada = Matrix<double>.Build.Dense(m, n);
        var medias = new double[n];
        
        for (int j = 0; j < n; j++)
        {
            medias[j] = matrizOriginal.Column(j).Mean();
            for (int i = 0; i < m; i++)
            {
                matrizCentralizada[i, j] = matrizOriginal[i, j] - medias[j];
            }
        }
        await Task.Delay(100);

        yield return ResultadoSSE<ResultadoPCA>.CriarProgressoParcial(40, "Calculando matriz de covariancia...");
        var covariancia = (matrizCentralizada.Transpose() * matrizCentralizada) / (m - 1);
        await Task.Delay(100);

        yield return ResultadoSSE<ResultadoPCA>.CriarProgressoParcial(70, "Calculando autovetores (Componentes Principais)...");
        var evd = covariancia.Evd();
        
        // Ordenar por autovalores decrescentes
        var pares = evd.EigenValues
            .Select((v, i) => new { Valor = v.Real, Vetor = evd.EigenVectors.Column(i) })
            .OrderByDescending(p => p.Valor)
            .ToList();

        var autovaloresTotal = pares.Sum(p => p.Valor);
        var componentes = pares.Take(componentesAlvo).Select(p => new Vetor(p.Vetor.ToArray())).ToArray();
        var varianciaExplicada = pares.Take(componentesAlvo).Select(p => p.Valor / autovaloresTotal).ToArray();
        
        var acumulada = new double[componentesAlvo];
        double soma = 0;
        for (int i = 0; i < componentesAlvo; i++)
        {
            soma += varianciaExplicada[i];
            acumulada[i] = soma;
        }

        yield return ResultadoSSE<ResultadoPCA>.CriarProgressoParcial(90, "Projetando dados no novo espaco...");
        var matrizComponentes = Matrix<double>.Build.DenseOfColumnArrays(componentes.Select(v => v.Componentes).ToArray());
        var pontosReduzidos = (matrizCentralizada * matrizComponentes).ToRowArrays()
            .Select(r => new Vetor(r)).ToArray();

        var resultado = new ResultadoPCA(componentes, varianciaExplicada, acumulada, pontosReduzidos);
        yield return ResultadoSSE<ResultadoPCA>.CriarConclusao(resultado);
    }
}
