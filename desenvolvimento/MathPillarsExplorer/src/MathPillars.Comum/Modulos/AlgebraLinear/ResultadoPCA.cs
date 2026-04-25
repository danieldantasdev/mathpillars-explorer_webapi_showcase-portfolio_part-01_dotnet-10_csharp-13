using MathPillars.Comum.Primitivos;

namespace MathPillars.Comum.Modulos.AlgebraLinear;

/// <summary>
/// Resultado da reducao de dimensionalidade via Analise de Componentes Principais (PCA).
/// </summary>
public record ResultadoPCA(
    Vetor[] ComponentesPrincipais,
    double[] VarianciaExplicadaPorComponente,
    double[] VarianciaExplicadaAcumulada,
    Vetor[] PontosReduzidos
);
