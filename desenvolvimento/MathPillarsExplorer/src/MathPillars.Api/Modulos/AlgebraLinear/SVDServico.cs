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
        
        // Calculamos a SVD. O MathNet por padrao pode retornar a versao "thin".
        // Para fins educacionais, queremos mostrar as dimensoes corretas de U (m x m) e VT (n x n).
        var svd = matrizMathNet.Svd(computeVectors: true);

        // U deve ser m x m na decomposicao completa
        var matrizU = ConverterDeMatrizMathNet(svd.U);
        
        // VT deve ser n x n na decomposicao completa
        var matrizVTransposta = ConverterDeMatrizMathNet(svd.VT);
        
        // W e a matriz Sigma. O MathNet retorna uma matriz diagonal de tamanho min(m,n) x min(m,n).
        // Vamos reconstruir a Sigma com as dimensoes originais m x n para ser fiel a teoria.
        var sigmaCompleta = Matrix<double>.Build.Dense(matrizEntrada.Linhas, matrizEntrada.Colunas);
        var valoresSingulares = svd.S.ToArray();
        for (int i = 0; i < valoresSingulares.Length; i++)
        {
            sigmaCompleta[i, i] = valoresSingulares[i];
        }
        var matrizSigma = ConverterDeMatrizMathNet(sigmaCompleta);

        var resultadoFinal = new ResultadoSVD(matrizU, matrizSigma, matrizVTransposta, valoresSingulares);
        
        yield return ResultadoSSE<ResultadoSVD>.CriarProgressoParcial(50, "Decomposicao concluida. Formatando tensores...");
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
