using MathPillars.Comum.Primitivos;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

public class JacobianaServico
{
    public Matriz CalcularJacobiana(Func<double[], double[]> funcaoVetorial, Vetor ponto, double h = 1e-6)
    {
        var dimIn = ponto.Componentes.Length;
        var pontoArray = ponto.Componentes.ToArray();
        
        var fZero = funcaoVetorial(pontoArray);
        var dimOut = fZero.Length;
        
        var jacobiana = new double[dimOut, dimIn];

        for (int j = 0; j < dimIn; j++)
        {
            var original = pontoArray[j];
            pontoArray[j] = original + h;
            var fH = funcaoVetorial(pontoArray);
            
            for (int i = 0; i < dimOut; i++)
            {
                jacobiana[i, j] = (fH[i] - fZero[i]) / h;
            }
            
            pontoArray[j] = original;
        }

        return new Matriz(jacobiana);
    }
}
