namespace MathPillars.Comum.Primitivos;

/// <summary>
/// Representa um escalar matematico real encapsulado para uso como tipo de retorno uniforme.
/// </summary>
public record Escalar(double Valor)
{
    public override string ToString() => Valor.ToString("G6");
}
