namespace MathPillars.Comum.Contratos;

// Algebra Linear
public record RequisicaoProdutoEscalar(double[] VetorA, double[] VetorB);
public record RequisicaoSimilaridade(double[] VetorA, double[] VetorB);
public record RequisicaoSVD(int Linhas, int Colunas, string ElementosJson);
public record RequisicaoPCA(int Linhas, int Colunas, string ElementosJson, int Componentes);
public record RequisicaoAutovetores(double[][] Elementos, int Dimensao);

// Calculo
public record RequisicaoGradiente(string FuncaoNome, double[] Ponto);
public record RequisicaoLossLandscape(string FuncaoNome, double MinX, double MaxX, double MinY, double MaxY);
public record RequisicaoJacobiana(string SistemaNome, double[] Ponto);
public record RequisicaoComparacaoOtimizadores(string FuncaoNome, double XInicial, double YInicial, double LearningRate, int Iteracoes);

// Probabilidade
public record RequisicaoBayes(double PriorH, double VerossimilhancaEH, double VerossimilhancaENaoH);
public record RequisicaoGaussiana(double Media, double DesvioPadrao, double Min, double Max, int Pontos);
public record RequisicaoEntropiaCruzada(double[] Real, double[] Predito);
public record RequisicaoMarkov(double[][] MatrizTransicao, int Passos, int EstadoInicial);
