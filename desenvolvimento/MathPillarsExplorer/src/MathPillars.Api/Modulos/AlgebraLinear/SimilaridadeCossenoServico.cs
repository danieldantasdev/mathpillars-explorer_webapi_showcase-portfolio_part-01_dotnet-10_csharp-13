using MathPillars.Comum.Primitivos;

namespace MathPillars.Api.Modulos.AlgebraLinear;

/// <summary>
/// Servico responsavel por calcular a similaridade de cosseno entre dois vetores.
/// Formaliza: similarity(A,B) = (A·B) / (||A|| * ||B||)
/// </summary>
public class SimilaridadeCossenoServico
{
    public Escalar CalcularSimilaridade(Vetor vetorA, Vetor vetorB)
    {
        var normaA = vetorA.CalcularNorma();
        var normaB = vetorB.CalcularNorma();

        if (normaA == 0 || normaB == 0)
            throw new InvalidOperationException("Nao e possivel calcular similaridade com vetor de norma zero.");

        var similaridade = vetorA.CalcularProdutoEscalarCom(vetorB) / (normaA * normaB);
        return new Escalar(Math.Clamp(similaridade, -1.0, 1.0));
    }

    public Escalar CalcularAnguloEmGraus(Vetor vetorA, Vetor vetorB)
    {
        var similaridade = CalcularSimilaridade(vetorA, vetorB);
        var anguloEmRadianos = Math.Acos(similaridade.Valor);
        return new Escalar(anguloEmRadianos * (180.0 / Math.PI));
    }
}
