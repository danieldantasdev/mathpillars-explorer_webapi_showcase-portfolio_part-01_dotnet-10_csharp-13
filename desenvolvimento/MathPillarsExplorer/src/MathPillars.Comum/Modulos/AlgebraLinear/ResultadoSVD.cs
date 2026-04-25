using MathPillars.Comum.Primitivos;

namespace MathPillars.Comum.Modulos.AlgebraLinear;

/// <summary>
/// Resultado completo da decomposicao SVD de uma matriz A = U * Sigma * V^T.
/// </summary>
public record ResultadoSVD(
    Matriz MatrizU,
    Matriz MatrizSigma,
    Matriz MatrizVTransposta,
    double[] ValoresSingulares
);
