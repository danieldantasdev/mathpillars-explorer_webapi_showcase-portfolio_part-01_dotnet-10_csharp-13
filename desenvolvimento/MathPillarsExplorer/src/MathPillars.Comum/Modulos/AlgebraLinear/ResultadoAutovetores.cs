using MathPillars.Comum.Primitivos;

namespace MathPillars.Comum.Modulos.AlgebraLinear;

/// <summary>
/// Resultado do calculo de autovetores e autovalores de uma matriz quadrada.
/// </summary>
public record ResultadoAutovetores(
    Vetor[] Autovetores,
    double[] Autovalores
);
