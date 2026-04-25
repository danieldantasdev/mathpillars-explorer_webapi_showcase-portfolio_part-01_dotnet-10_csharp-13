namespace MathPillars.Comum.Contratos;

// Algebra Linear
public record RequisicaoProdutoEscalar(double[] VetorA, double[] VetorB);
public record RequisicaoSimilaridade(double[] VetorA, double[] VetorB);
public record RequisicaoSVD(double[][] Elementos, int Linhas, int Colunas);
public record RequisicaoPCA(double[][] Pontos, int NumeroDimensoesAlvo);
public record RequisicaoAutovetores(double[][] Elementos, int Dimensao);

// Calculo
public record RequisicaoGradiente(string FuncaoNome, double[] Ponto);
public record RequisicaoLossLandscape(string FuncaoNome, double MinX, double MaxX, double MinY, double MaxY);

// Probabilidade
public record RequisicaoBayes(double PriorH, double VerossimilhancaEH, double VerossimilhancaENaoH);
