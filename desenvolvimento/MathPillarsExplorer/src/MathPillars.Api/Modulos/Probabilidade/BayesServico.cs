using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.Probabilidade;

namespace MathPillars.Api.Modulos.Probabilidade;

public class BayesServico
{
    public ResultadoBayes CalcularPosterior(double priorH, double verossimilhancaEH, double verossimilhancaENaoH)
    {
        var priorNaoH = 1.0 - priorH;
        var evidencia = (verossimilhancaEH * priorH) + (verossimilhancaENaoH * priorNaoH);
        
        if (evidencia == 0) throw new InvalidOperationException("Evidencia zero.");

        var posterior = (verossimilhancaEH * priorH) / evidencia;
        return new ResultadoBayes(posterior, priorH, verossimilhancaEH, evidencia);
    }
}
