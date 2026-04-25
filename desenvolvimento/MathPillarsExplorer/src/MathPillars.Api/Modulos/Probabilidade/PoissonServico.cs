namespace MathPillars.Api.Modulos.Probabilidade;

/// <summary>
/// Servico responsavel por calculos relacionados a Distribuicao de Poisson.
/// </summary>
public class PoissonServico
{
    public double CalcularProbabilidade(double lambda, int k)
    {
        if (lambda < 0 || k < 0) return 0;
        
        // P(X = k) = (e^-lambda * lambda^k) / k!
        return (Math.Exp(-lambda) * Math.Pow(lambda, k)) / Fatorial(k);
    }

    private double Fatorial(int n)
    {
        if (n <= 1) return 1;
        double resultado = 1;
        for (int i = 2; i <= n; i++) resultado *= i;
        return resultado;
    }
}
