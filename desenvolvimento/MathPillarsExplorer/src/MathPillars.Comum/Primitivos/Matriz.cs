namespace MathPillars.Comum.Primitivos;

/// <summary>
/// Representa uma matriz matematica bidimensional com operacoes fundamentais.
/// </summary>
public record Matriz
{
    public int Linhas { get; init; }
    public int Colunas { get; init; }
    public double[,] Elementos { get; init; }

    public Matriz(double[,] elementos)
    {
        Elementos = elementos;
        Linhas = elementos.GetLength(0);
        Colunas = elementos.GetLength(1);
    }

    public bool EhQuadrada() => Linhas == Colunas;

    public Matriz ObterTransposta()
    {
        var transposta = new double[Colunas, Linhas];
        for (var linha = 0; linha < Linhas; linha++)
            for (var coluna = 0; coluna < Colunas; coluna++)
                transposta[coluna, linha] = Elementos[linha, coluna];
        return new Matriz(transposta);
    }

    public Matriz MultiplicarPor(Matriz outra)
    {
        if (Colunas != outra.Linhas)
            throw new ArgumentException("Numero de colunas da primeira matriz deve ser igual ao numero de linhas da segunda.");

        var resultado = new double[Linhas, outra.Colunas];
        for (var i = 0; i < Linhas; i++)
            for (var j = 0; j < outra.Colunas; j++)
                for (var k = 0; k < Colunas; k++)
                    resultado[i, j] += Elementos[i, k] * outra.Elementos[k, j];

        return new Matriz(resultado);
    }
}
