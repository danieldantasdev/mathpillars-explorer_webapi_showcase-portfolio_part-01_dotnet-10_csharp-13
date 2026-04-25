using MathPillars.Comum.Primitivos;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

public class GradienteServico
{
    public Vetor CalcularGradiente(Func<double[], double> funcao, Vetor ponto, double h = 1e-6)
    {
        var dimensoes = ponto.Componentes.Length;
        var gradiente = new double[dimensoes];
        var pontoArray = ponto.Componentes.ToArray();

        for (int i = 0; i < dimensoes; i++)
        {
            var original = pontoArray[i];
            
            pontoArray[i] = original + h;
            var fSuperior = funcao(pontoArray);
            
            pontoArray[i] = original - h;
            var fInferior = funcao(pontoArray);
            
            gradiente[i] = (fSuperior - fInferior) / (2 * h);
            pontoArray[i] = original;
        }

        return new Vetor(gradiente);
    }
}
