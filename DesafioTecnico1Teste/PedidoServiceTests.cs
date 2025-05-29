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

    [Fact]
    public async Task DadoUmPedidoCriadoValorDeProdutoNoEstoqueDeveSerAbatido()
    {
        // Arrange - cria cliente
        var clienteResponse = await _client.GetAsync("/api/Cliente");
        clienteResponse.EnsureSuccessStatusCode();
        var clientes = await clienteResponse.Content.ReadFromJsonAsync<List<Cliente>>();
        Assert.NotEmpty(clientes);
        var cliente = clientes.First();


        //Cria Produto 
        var produtoDto = new
        {
            Nome = "Produto Teste",
            Descricao = "Descrição Produto Teste",
            Preco = 10,
            Estoque = 100,
        };
        var response = await _client.PostAsJsonAsync("/api/Produto", produtoDto);
        response.EnsureSuccessStatusCode();
        var produto = await response.Content.ReadFromJsonAsync<Produto>();

        

        // Cria pedido
        var pedidoDto = new
        {
            ClientId = cliente.Id,
            DataPedido = DateTime.UtcNow,
            Status = 0,
            Itens = new List<object>
            {
            }
        };


        var createResponse = await _client.PostAsJsonAsync("/api/Pedido", pedidoDto);
        createResponse.EnsureSuccessStatusCode();

        // Buscar pedido criado
        var pedidosResponse = await _client.GetAsync("/api/Pedido");
        var pedidos = await pedidosResponse.Content.ReadFromJsonAsync<List<Pedido>>();
        var pedidoCriado = pedidos.Last(); // Pega o último criado

        //cria item Pedido
        var itemDto = new
        {
            ProdutoId = produto.Id,
            PedidoId = pedidoCriado.Id,
            Quantidade = 10
        };

        var itemResponse = await _client.PostAsJsonAsync("/api/ItemPedido", itemDto);
        itemResponse.EnsureSuccessStatusCode();

        var produtoResponse = await _client.GetAsync($"/api/Produto/{produto.Id}");
        produtoResponse.EnsureSuccessStatusCode();
        var produtoAtualizado = await produtoResponse.Content.ReadFromJsonAsync<Produto>();

        Assert.Equal(produto.Estoque - itemDto.Quantidade, produtoAtualizado.Estoque);


    }

    //ultimo teste verificar se alterar o status para cancelado os produtos de estoque sao repostos
    [Fact]
    public async Task DadoAlteracaoDoPedidoParaCanceladoItensProdutosDevemSerZeradosERespostoEmEstoque()
    {
        // Arrange - cria cliente
        var clienteResponse = await _client.GetAsync("/api/Cliente");
        clienteResponse.EnsureSuccessStatusCode();
        var clientes = await clienteResponse.Content.ReadFromJsonAsync<List<Cliente>>();
        Assert.NotEmpty(clientes);
        var cliente = clientes.First();

        //Cria Produto 
        var produtoDto = new
        {
            Nome = "Produto Teste",
            Descricao = "Descrição Produto Teste",
            Preco = 10,
            Estoque = 100,
        };
        var response = await _client.PostAsJsonAsync("/api/Produto", produtoDto);
        response.EnsureSuccessStatusCode();
        var produto = await response.Content.ReadFromJsonAsync<Produto>();



        // Cria pedido
        var pedidoDto = new
        {
            ClientId = cliente.Id,
            DataPedido = DateTime.UtcNow,
            Status = 0,
            Itens = new List<object>
            {
            }
        };


        var createResponse = await _client.PostAsJsonAsync("/api/Pedido", pedidoDto);
        createResponse.EnsureSuccessStatusCode();

        // Buscar pedido criado
        var pedidosResponse = await _client.GetAsync("/api/Pedido");
        var pedidos = await pedidosResponse.Content.ReadFromJsonAsync<List<Pedido>>();
        var pedidoCriado = pedidos.Last(); // Pega o último criado

        //cria item Pedido
        var itemDto = new
        {
            ProdutoId = produto.Id,
            PedidoId = pedidoCriado.Id,
            Quantidade = 10
        };

        var itemResponse = await _client.PostAsJsonAsync("/api/ItemPedido", itemDto);
        itemResponse.EnsureSuccessStatusCode();

        var patchDto = new
        {
            Status = 2 // para Cancelar
        };
        var statusReponse = await _client.PatchAsJsonAsync($"/api/Pedido/{pedidoCriado.Id}/status", patchDto);
        statusReponse.EnsureSuccessStatusCode();

        //validar quantidade produto em estoque
        var responseProdutoAlterado = await _client.GetAsync($"/api/Produto/{produto.Id}");
        response.EnsureSuccessStatusCode();
        var produtoAlterado = await responseProdutoAlterado.Content.ReadFromJsonAsync<Produto>();

        Assert.Equal(produtoDto.Estoque, produtoAlterado.Estoque);
    }
}
