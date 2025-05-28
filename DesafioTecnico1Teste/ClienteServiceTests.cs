using DesafioTecnico1.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DesafioTecnico1Teste;

[Collection("Api Test Collection")]
public class ClienteServiceTests 
{
    private readonly HttpClient _client;

    public ClienteServiceTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task DadoUmGetDeveRetornarStatus200OK_EListaDeClientes()
    {
        var url = "/api/Cliente";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content));

        var clientes = JsonSerializer.Deserialize<List<object>>(content);
        Assert.NotNull(clientes);

    }

    [Fact]
    public async Task DadoUmPostDeveRetornarStatus201Created()
    {
        // Arrange
        var url = "/api/Cliente";

        var clienteDto = new
        {
            Email = "joao.silva@example.com",
            Telefone = "11999999999",
            DataCadastro = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync(url, clienteDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location;
        Assert.NotNull(location);
    }

    [Fact]
    public async Task DadoUmDeleteDeveRemoverClienteRetornandoStatus200OK()
    {
        // Arrange - cria cliente para ter ID
        var createUrl = "/api/Cliente";
        var clienteDto = new
        {
            Email = "maria.silva@example.com",
            Telefone = "11988888888",
            DataCadastro = DateTime.UtcNow
        };

        var createResponse = await _client.PostAsJsonAsync(createUrl, clienteDto);
        createResponse.EnsureSuccessStatusCode();

        var createdCliente = await createResponse.Content.ReadFromJsonAsync<Cliente>();

        Assert.NotNull(createdCliente);
        Assert.NotEqual(Guid.Empty, createdCliente.Id);

        // Act - remove o cliente criado
        var deleteUrl = $"/api/Cliente/{createdCliente.Id}";
        var deleteResponse = await _client.DeleteAsync(deleteUrl);

        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }
}
