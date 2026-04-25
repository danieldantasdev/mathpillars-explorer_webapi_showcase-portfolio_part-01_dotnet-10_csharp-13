using FluentAssertions;
using MathPillars.Comum.Primitivos;
using MathPillars.Api.Modulos.AlgebraLinear;
using Xunit;

namespace MathPillars.Testes.AlgebraLinear;

public class ProdutoEscalarServicoTestes
{
    private readonly ProdutoEscalarServico _servico;

    public ProdutoEscalarServicoTestes()
    {
        _servico = new ProdutoEscalarServico();
    }

    [Fact]
    public void CalcularProdutoEscalar_DeveRetornarValorCorreto()
    {
        // Arrange
        var vetorA = new Vetor(new[] { 1.0, 2.0, 3.0 });
        var vetorB = new Vetor(new[] { 4.0, 5.0, 6.0 });
        var esperado = 1 * 4 + 2 * 5 + 3 * 6; // 32

        // Act
        var resultado = _servico.CalcularProdutoEscalar(vetorA, vetorB);

        // Assert
        resultado.Valor.Should().Be(esperado);
    }

    [Fact]
    public void CalcularProjecao_DeveRetornarVetorCorreto()
    {
        // Arrange
        var vetorA = new Vetor(new[] { 3.0, 1.0 });
        var vetorB = new Vetor(new[] { 1.0, 0.0 });
        var esperado = new[] { 3.0, 0.0 };

        // Act
        var resultado = _servico.CalcularProjecao(vetorA, vetorB);

        // Assert
        resultado.Componentes.Should().Equal(esperado);
    }
}
