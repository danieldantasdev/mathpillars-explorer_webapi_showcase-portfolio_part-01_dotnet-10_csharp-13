namespace MathPillars.Comum.Primitivos;

/// <summary>
/// Representa um vetor matematico de dimensao arbitraria com operacoes fundamentais de algebra linear.
/// </summary>
public record Vetor
{
    public int Dimensoes { get; init; }
    public double[] Componentes { get; init; }

    public Vetor(double[] componentes)
    {
        Componentes = componentes;
        Dimensoes = componentes.Length;
    }

    public double CalcularNorma()
    {
        return Math.Sqrt(Componentes.Sum(componente => componente * componente));
    }

    public Vetor Normalizar()
    {
        var norma = CalcularNorma();
        if (norma == 0) throw new InvalidOperationException("Nao e possivel normalizar vetor nulo.");
        return new Vetor(Componentes.Select(c => c / norma).ToArray());
    }

    public double CalcularProdutoEscalarCom(Vetor outro)
    {
        if (Dimensoes != outro.Dimensoes)
            throw new ArgumentException("Vetores devem ter a mesma dimensao para calcular produto escalar.");

        return Componentes.Zip(outro.Componentes, (a, b) => a * b).Sum();
    }
}
