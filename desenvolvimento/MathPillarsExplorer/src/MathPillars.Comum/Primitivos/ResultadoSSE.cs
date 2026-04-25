namespace MathPillars.Comum.Primitivos;

/// <summary>
/// Contrato de streaming SSE utilizado por todos os endpoints de calculo assincrono.
/// Transporta progresso parcial e o resultado final de forma progressiva.
/// </summary>
public record ResultadoSSE<T>(
    int ProgressoPercentual,
    string MensagemProgresso,
    T? DadosParciais,
    bool Concluido
)
{
    public static ResultadoSSE<T> CriarProgressoInicial(string mensagem) =>
        new(0, mensagem, default, false);

    public static ResultadoSSE<T> CriarProgressoParcial(int percentual, string mensagem) =>
        new(percentual, mensagem, default, false);

    public static ResultadoSSE<T> CriarConclusao(T dados) =>
        new(100, "Calculo concluido com sucesso.", dados, true);
}
