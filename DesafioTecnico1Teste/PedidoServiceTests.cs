using DesafioTecnico1.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace DesafioTecnico1Teste;

[Collection("Api Test Collection")]
public class PedidoServiceTests
{
    private readonly HttpClient _client;

    public PedidoServiceTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task DadoUmPostPedidoComClienteValidoDeveRetornarStatus201Created()
    {
        // Arrange - pegar um cliente válido
        var clienteResponse = await _client.GetAsync("/api/Cliente");
        clienteResponse.EnsureSuccessStatusCode();
        var clientes = await clienteResponse.Content.ReadFromJsonAsync<List<Cliente>>();
        Assert.NotEmpty(clientes);

        var cliente = clientes.First();

        var pedidoDto = new
        {
            ClientId = cliente.Id,
            DataPedido = DateTime.UtcNow,
            Itens = new List<object>
            {
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Pedido", pedidoDto);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DadoUmPostPedidoComClienteInvalidoDeveRetornarErro()
    {
        // Arrange - cliente inexistente
        var pedidoDto = new
        {
            ClientId = Guid.NewGuid(), // ID que não existe
            DataPedido = DateTime.UtcNow,
            Status = 0,
            Itens = new List<object>
             {
             }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Pedido", pedidoDto);

        // Assert - depende da sua implementação:
        // se lançar exceção → status 500, se validar → 400 ou similar
        Assert.True(
            response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
            response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
    }
}
