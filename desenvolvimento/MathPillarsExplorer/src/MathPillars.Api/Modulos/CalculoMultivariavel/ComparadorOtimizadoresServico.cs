using MathPillars.Comum.Primitivos;
using MathPillars.Comum.Modulos.CalculoMultivariavel;

namespace MathPillars.Api.Modulos.CalculoMultivariavel;

public class ComparadorOtimizadoresServico
{
    private readonly GradienteServico _gradienteServico;

    public ComparadorOtimizadoresServico(GradienteServico gradienteServico)
    {
        _gradienteServico = gradienteServico;
    }

    public async IAsyncEnumerable<ResultadoSSE<ResultadoComparacaoOtimizadores>> CompararCaminhosSSE(
        string funcaoNome, double xInicial, double yInicial, double learningRate, int iteracoes)
    {
        Func<double[], double> funcao = GetFuncao(funcaoNome);
        
        var trajetoriaAdamW = new List<PontoSuperficie3D>();
        var trajetoriaSophia = new List<PontoSuperficie3D>();
        var perdasAdamW = new List<double>();
        var perdasSophia = new List<double>();

        // AdamW State
        double[] thetaAdamW = { xInicial, yInicial };
        double[] mAdamW = { 0, 0 };
        double[] vAdamW = { 0, 0 };
        double beta1 = 0.9, beta2 = 0.999, eps = 1e-8, wd = 0.01;

        // Sophia State
        double[] thetaSophia = { xInicial, yInicial };
        double[] mSophia = { 0, 0 };
        double[] hSophia = { 1, 1 }; // Hessian estimate
        double rho = 0.9, gamma = 0.1;

        for (int t = 1; t <= iteracoes; t++)
        {
            // Update AdamW
            var gradAdamW = _gradienteServico.CalcularGradiente(funcao, new Vetor(thetaAdamW)).Componentes;
            for (int i = 0; i < 2; i++)
            {
                mAdamW[i] = beta1 * mAdamW[i] + (1 - beta1) * gradAdamW[i];
                vAdamW[i] = beta2 * vAdamW[i] + (1 - beta2) * gradAdamW[i] * gradAdamW[i];
                double mHat = mAdamW[i] / (1 - Math.Pow(beta1, t));
                double vHat = vAdamW[i] / (1 - Math.Pow(beta2, t));
                thetaAdamW[i] = thetaAdamW[i] - learningRate * (mHat / (Math.Sqrt(vHat) + eps) + wd * thetaAdamW[i]);
            }
            double zAdamW = funcao(thetaAdamW);
            trajetoriaAdamW.Add(new PontoSuperficie3D(thetaAdamW[0], thetaAdamW[1], zAdamW));
            perdasAdamW.Add(zAdamW);

            // Update Sophia (Simplified)
            var gradSophia = _gradienteServico.CalcularGradiente(funcao, new Vetor(thetaSophia)).Componentes;
            for (int i = 0; i < 2; i++)
            {
                mSophia[i] = rho * mSophia[i] + (1 - rho) * gradSophia[i];
                
                // Estimate Hessian diagonal (very simplified)
                double hVal = CalcularHessianaDiagonal(funcao, thetaSophia, i);
                hSophia[i] = rho * hSophia[i] + (1 - rho) * Math.Max(0, hVal);
                
                double update = mSophia[i] / Math.Max(gamma * hSophia[i], eps);
                thetaSophia[i] = thetaSophia[i] - learningRate * Math.Clamp(update, -1, 1);
            }
            double zSophia = funcao(thetaSophia);
            trajetoriaSophia.Add(new PontoSuperficie3D(thetaSophia[0], thetaSophia[1], zSophia));
            perdasSophia.Add(zSophia);

            if (t % Math.Max(1, iteracoes / 10) == 0)
            {
                yield return ResultadoSSE<ResultadoComparacaoOtimizadores>.CriarProgressoParcial(
                    t * 100 / iteracoes, 
                    $"Otimizando... Passo {t}/{iteracoes}");
            }
        }

        var resultado = new ResultadoComparacaoOtimizadores(
            trajetoriaAdamW.ToArray(), 
            trajetoriaSophia.ToArray(), 
            perdasAdamW.ToArray(), 
            perdasSophia.ToArray());

        yield return ResultadoSSE<ResultadoComparacaoOtimizadores>.CriarConclusao(resultado);
    }

    private double CalcularHessianaDiagonal(Func<double[], double> f, double[] x, int i, double h = 1e-4)
    {
        double original = x[i];
        double f0 = f(x);
        x[i] = original + h;
        double f1 = f(x);
        x[i] = original - h;
        double f2 = f(x);
        x[i] = original;
        return (f1 - 2 * f0 + f2) / (h * h);
    }

    private Func<double[], double> GetFuncao(string nome)
    {
        return nome.ToLower() switch
        {
            "rosenbrock" => p => Math.Pow(1 - p[0], 2) + 100 * Math.Pow(p[1] - p[0] * p[0], 2),
            "quadratica" => p => p[0] * p[0] + p[1] * p[1],
            _ => p => p[0] * p[0] + p[1] * p[1]
        };
    }
}
