using MathNet.Numerics.LinearAlgebra;
using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.AlgebraLinear;

namespace MathPillars.Api.Modulos.AlgebraLinear;

/// <summary>
/// Servico responsavel por executar a Decomposicao em Valores Singulares (SVD) com streaming SSE.
/// Formaliza: A = U * Sigma * V^T
/// </summary>
public class SVDServico
{
    private const int LimiteDimensaoMaxima = 1000;

    public async IAsyncEnumerable<ResultadoSSE<ResultadoSVD>> DecomporComStreamingSSE(Matriz matrizEntrada)
    {
        var matrizMathNet = ConverterParaMatrizMathNet(matrizEntrada);
        var svd = matrizMathNet.Svd(computeVectors: true);

        var matrizU = ConverterDeMatrizMathNet(svd.U);
        var matrizSigma = ConverterDeMatrizMathNet(svd.W);
        var matrizVTransposta = ConverterDeMatrizMathNet(svd.VT);
        var valoresSingulares = svd.S.ToArray();

        var resultadoFinal = new ResultadoSVD(matrizU, matrizSigma, matrizVTransposta, valoresSingulares);
        yield return ResultadoSSE<ResultadoSVD>.CriarConclusao(resultadoFinal);
    }

    private static Matrix<double> ConverterParaMatrizMathNet(Matriz matriz)
    {
        var matrizMathNet = Matrix<double>.Build.Dense(matriz.Linhas, matriz.Colunas);
        for (var linha = 0; linha < matriz.Linhas; linha++)
            for (var coluna = 0; coluna < matriz.Colunas; coluna++)
                matrizMathNet[linha, coluna] = matriz.Elementos[linha][coluna];
        return matrizMathNet;
    }

    private static Matriz ConverterDeMatrizMathNet(Matrix<double> matrizMathNet)
    {
        var elementos = new double[matrizMathNet.RowCount, matrizMathNet.ColumnCount];
        for (var linha = 0; linha < matrizMathNet.RowCount; linha++)
            for (var coluna = 0; coluna < matrizMathNet.ColumnCount; coluna++)
                elementos[linha, coluna] = matrizMathNet[linha, coluna];
        return Matriz.DeArrayBidimensional(elementos);
    }
}
