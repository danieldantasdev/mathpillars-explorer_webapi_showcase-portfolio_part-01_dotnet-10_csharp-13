using MathPillars.Comum.Primitivos;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

/// <summary>
/// Servico responsavel por calcular a matriz Hessiana de uma funcao escalar.
/// </summary>
public class HessianaServico
{
    public Matriz CalcularHessiana(Func<double[], double> funcao, Vetor ponto, double h = 1e-4)
    {
        var n = ponto.Componentes.Length;
        var pontoArray = ponto.Componentes.ToArray();
        var hessiana = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    // Segunda derivada parcial pura
                    double original = pontoArray[i];
                    double f0 = funcao(pontoArray);
                    
                    pontoArray[i] = original + h;
                    double f1 = funcao(pontoArray);
                    
                    pontoArray[i] = original - h;
                    double f2 = funcao(pontoArray);
                    
                    hessiana[i, j] = (f1 - 2 * f0 + f2) / (h * h);
                    pontoArray[i] = original;
                }
                else
                {
                    // Derivada parcial mista
                    double originalI = pontoArray[i];
                    double originalJ = pontoArray[j];

                    pontoArray[i] = originalI + h;
                    pontoArray[j] = originalJ + h;
                    double f11 = funcao(pontoArray);

                    pontoArray[i] = originalI + h;
                    pontoArray[j] = originalJ - h;
                    double f12 = funcao(pontoArray);

                    pontoArray[i] = originalI - h;
                    pontoArray[j] = originalJ + h;
                    double f21 = funcao(pontoArray);

                    pontoArray[i] = originalI - h;
                    pontoArray[j] = originalJ - h;
                    double f22 = funcao(pontoArray);

                    hessiana[i, j] = (f11 - f12 - f21 + f22) / (4 * h * h);
                    
                    pontoArray[i] = originalI;
                    pontoArray[j] = originalJ;
                }
            }
        }

        return Matriz.DeArrayBidimensional(hessiana);
    }
}
