using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace DesafioTecnico1Teste;

public class PedidoServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidoServiceTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DadoUmGetDeveRetornarStatus200OK()
    {
        //arrange
        //act
        var response = await _client.GetAsync("/api/Pedido");
        //assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    }
}
