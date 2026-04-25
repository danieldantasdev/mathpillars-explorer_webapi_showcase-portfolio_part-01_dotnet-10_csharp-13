using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.AlgebraLinear;
using MathNet.Numerics.LinearAlgebra;

namespace MathPillars.Api.Modulos.AlgebraLinear;

public class AutovetoresServico
{
    public ResultadoAutovetores CalcularAutovetoresEAutovalores(Matriz matriz)
    {
        var matrizMathNet = Matrix<double>.Build.DenseOfArray(matriz.Elementos);
        var evd = matrizMathNet.Evd();
        
        var autovalores = evd.EigenValues.Select(c => c.Real).ToArray();
        var autovetoresMatriz = evd.EigenVectors;
        
        var autovetores = new Vetor[autovetoresMatriz.ColumnCount];
        for (int i = 0; i < autovetoresMatriz.ColumnCount; i++)
        {
            autovetores[i] = new Vetor(autovetoresMatriz.Column(i).ToArray());
        }

        return new ResultadoAutovetores(autovetores, autovalores);
    }
}
