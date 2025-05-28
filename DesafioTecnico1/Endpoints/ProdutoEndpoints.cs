using Microsoft.EntityFrameworkCore;
using DesafioTecnico1.Data;
using DesafioTecnico1.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using AutoMapper;
using DesafioTecnico1.DTOs;
namespace DesafioTecnico1.Endpoints;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Produto").WithTags(nameof(Produto));

        group.MapGet("/", async (DesafioTecnicoContext db) =>
        {
            return await db.Produto.ToListAsync();
        })
        .WithName("GetAllProdutos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Produto>, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            return await db.Produto.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Produto model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProdutoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, ProdutoDto produtoDto, DesafioTecnicoContext db, IMapper mapper) =>
        {
            var produto = mapper.Map<Produto>(produtoDto);
            var affected = await db.Produto
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    //.SetProperty(m => m.Id, produto.Id)
                    .SetProperty(m => m.Nome, produto.Nome)
                    .SetProperty(m => m.Descricao, produto.Descricao)
                    .SetProperty(m => m.Preco, produto.Preco)
                    .SetProperty(m => m.Estoque, produto.Estoque)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProduto")
        .WithOpenApi();

        group.MapPost("/", async (ProdutoDto produtoDto, DesafioTecnicoContext db, IMapper mapper) =>
        {
            var produto = mapper.Map<Produto>(produtoDto);
            db.Produto.Add(produto);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Produto/{produto.Id}",produto);
        })
        .WithName("CreateProduto")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            var affected = await db.Produto
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProduto")
        .WithOpenApi();
    }
}
