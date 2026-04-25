using MathPillars.Comum.Primitivos;

namespace MathPillars.Api.Modulos.AlgebraLinear;

/// <summary>
/// Servico responsavel por calcular o produto escalar entre dois vetores e a projecao de um sobre o outro.
/// </summary>
public class ProdutoEscalarServico
{
    public Escalar CalcularProdutoEscalar(Vetor vetorA, Vetor vetorB)
    {
        var resultado = vetorA.CalcularProdutoEscalarCom(vetorB);
        return new Escalar(resultado);
    }

    public Vetor CalcularProjecao(Vetor vetorA, Vetor vetorB)
    {
        var produtoEscalar = vetorA.CalcularProdutoEscalarCom(vetorB);
        var normaBQuadrado = vetorB.CalcularNorma() * vetorB.CalcularNorma();

        if (normaBQuadrado == 0)
            throw new InvalidOperationException("Nao e possivel calcular projecao sobre vetor nulo.");

        var escalar = produtoEscalar / normaBQuadrado;
        return new Vetor(vetorB.Componentes.Select(c => c * escalar).ToArray());
    }
}
