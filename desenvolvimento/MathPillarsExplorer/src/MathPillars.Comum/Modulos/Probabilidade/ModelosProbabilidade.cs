namespace MathPillars.Comum.Modulos.Probabilidade;

/// <summary>
/// Resultado do calculo do Teorema de Bayes.
/// </summary>
public record ResultadoBayes(
    double ProbabilidadePosterior,
    double ProbabilidadePrior,
    double Verossimilhanca,
    double Evidencia
);

/// <summary>
/// Representa um ponto em uma curva de distribuicao gaussiana.
/// </summary>
public record PontoGaussiana(double X, double Y);

/// <summary>
/// Resultado do calculo de Entropia Cruzada entre duas distribuicoes.
/// </summary>
public record ResultadoEntropiaCruzada(
    double EntropiaCruzada,
    double EntropiaShannonDistribuicaoA,
    double DivergenciaKL
);
