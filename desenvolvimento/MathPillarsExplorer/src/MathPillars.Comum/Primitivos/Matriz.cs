namespace MathPillars.Comum.Primitivos;

/// <summary>
/// Representa uma matriz matematica bidimensional com operacoes fundamentais.
/// </summary>
public record Matriz
{
    public int Linhas { get; init; }
    public int Colunas { get; init; }
    public double[][] Elementos { get; init; }

    public Matriz(double[][] elementos)
    {
        Elementos = elementos;
        Linhas = elementos.Length;
        Colunas = elementos.Length > 0 ? elementos[0].Length : 0;
    }

    public static Matriz DeArrayBidimensional(double[,] array)
    {
        int linhas = array.GetLength(0);
        int colunas = array.GetLength(1);
        var elementos = new double[linhas][];
        for (int i = 0; i < linhas; i++)
        {
            elementos[i] = new double[colunas];
            for (int j = 0; j < colunas; j++)
            {
                elementos[i][j] = array[i, j];
            }
        }
        return new Matriz(elementos);
    }

    public bool EhQuadrada() => Linhas == Colunas;

    public Matriz ObterTransposta()
    {
        var transposta = new double[Colunas][];
        for (int i = 0; i < Colunas; i++)
        {
            transposta[i] = new double[Linhas];
            for (int j = 0; j < Linhas; j++)
            {
                transposta[i][j] = Elementos[j][i];
            }
        }
        return new Matriz(transposta);
    }

    public Matriz MultiplicarPor(Matriz outra)
    {
        if (Colunas != outra.Linhas)
            throw new ArgumentException("Numero de colunas da primeira matriz deve ser igual ao numero de linhas da segunda.");

        var resultado = new double[Linhas][];
        for (var i = 0; i < Linhas; i++)
        {
            resultado[i] = new double[outra.Colunas];
            for (var j = 0; j < outra.Colunas; j++)
            {
                for (var k = 0; k < Colunas; k++)
                    resultado[i][j] += Elementos[i][k] * outra.Elementos[k][j];
            }
        }

        return new Matriz(resultado);
    }
}
