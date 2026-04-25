using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;

namespace MathPillars.Api.Modulos.Probabilidade;

public class EntropiaCruzadaServico
{
    public ResultadoEntropiaCruzada Calcular(double[] real, double[] predito)
    {
        var soma = 0.0;
        for (int i = 0; i < real.Length; i++)
        {
            var p = Math.Max(predito[i], 1e-15);
            soma += real[i] * Math.Log(p);
        }

        return new ResultadoEntropiaCruzada(-soma, 0, 0);
    }
}
