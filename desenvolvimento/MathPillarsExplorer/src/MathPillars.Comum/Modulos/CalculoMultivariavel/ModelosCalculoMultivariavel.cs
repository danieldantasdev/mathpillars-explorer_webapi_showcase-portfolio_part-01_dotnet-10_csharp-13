using MathPillars.Comum.Primitivos;

namespace MathPillars.Comum.Modulos.CalculoMultivariavel;

/// <summary>
/// Representa um ponto tridimensional em uma superficie de funcao de perda (Loss Landscape).
/// </summary>
public record PontoSuperficie3D(double X, double Y, double Z);

/// <summary>
/// Resultado da comparacao entre os otimizadores AdamW e Sophia sobre a mesma Loss Landscape.
/// </summary>
public record ResultadoComparacaoOtimizadores(
    PontoSuperficie3D[] TrajetoriaAdamW,
    PontoSuperficie3D[] TrajetoriaSophia,
    double[] PerdasAdamW,
    double[] PerdasSophia
);
