using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.AlgebraLinear;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace MathPillars.Api.Modulos.AlgebraLinear;

public class PCAServico
{
    public async IAsyncEnumerable<ResultadoSSE<ResultadoPCA>> CalcularPCAComStreamingSSE(double[][] dados, int componentesAlvo)
    {
        var matrizDados = Matrix<double>.Build.DenseOfRows(dados);
        
        // Passo 1: Centralizacao
        var medias = new double[matrizDados.ColumnCount];
        for (int j = 0; j < matrizDados.ColumnCount; j++)
        {
            medias[j] = matrizDados.Column(j).Mean();
            for (int i = 0; i < matrizDados.RowCount; i++)
                matrizDados[i, j] -= medias[j];
        }
        
        yield return ResultadoSSE<ResultadoPCA>.CriarProgressoParcial(10, "Dados centralizados.");

        // Passo 2: Matriz de Covariancia
        var covariancia = (matrizDados.Transpose() * matrizDados) / (matrizDados.RowCount - 1);
        yield return ResultadoSSE<ResultadoPCA>.CriarProgressoParcial(40, "Matriz de covariancia calculada.");

        // Passo 3: EVD
        var evd = covariancia.Evd();
        var autovalores = evd.EigenValues.Select(c => c.Real).ToArray();
        
        var varianciaExplicada = autovalores.OrderByDescending(v => v).Take(componentesAlvo).Sum() / autovalores.Sum();
        
        var resultado = new ResultadoPCA(new Vetor[0], autovalores, new double[componentesAlvo], new Vetor[0]);
        yield return ResultadoSSE<ResultadoPCA>.CriarConclusao(resultado);
    }
}
