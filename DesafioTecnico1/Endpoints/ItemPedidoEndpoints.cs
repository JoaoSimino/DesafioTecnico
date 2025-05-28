using Microsoft.EntityFrameworkCore;
using DesafioTecnico1.Data;
using DesafioTecnico1.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using AutoMapper;
using DesafioTecnico1.DTOs;
using DesafioTecnico1.Exceptions;
namespace DesafioTecnico1.Endpoints;

public static class ItemPedidoEndpoints
{
    public static void MapItemPedidoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ItemPedido").WithTags(nameof(ItemPedido));

        group.MapGet("/", async (DesafioTecnicoContext db) =>
        {
            return await db.ItemPedido.ToListAsync();
        })
        .WithName("GetAllItemPedidos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ItemPedido>, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            return await db.ItemPedido.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ItemPedido model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetItemPedidoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, ItemPedido itemPedido, DesafioTecnicoContext db) =>
        {
            var affected = await db.ItemPedido
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    //.SetProperty(m => m.Id, itemPedido.Id)
                    .SetProperty(m => m.ProdutoId, itemPedido.ProdutoId)
                    .SetProperty(m => m.Quantidade, itemPedido.Quantidade)
                    .SetProperty(m => m.PrecoUnitario, itemPedido.PrecoUnitario)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateItemPedido")
        .WithOpenApi();

        group.MapPost("/", async (ItemPedidoDto itemPedidoDto, DesafioTecnicoContext db, IMapper mapper) =>
        {

            var itemPedido = mapper.Map<ItemPedido>(itemPedidoDto);

            //validar se o produtoId é valido
            if (db.Produto.Find(itemPedido.ProdutoId) is null) 
            {
                throw new ItemPedidoExceptions("O ID do Produto em questão não foi encontrado!!");
            }

            //validar se o pedidoId é valido
            if (db.Pedido.Find(itemPedido.PedidoId) is null)
            {
                throw new ItemPedidoExceptions("O ID do Pedido em questão não foi encontrado!!");
            }

            //validar se esta disponivel em estoque 
            var produto = db.Produto.Find(itemPedido.ProdutoId);
            if (produto.Estoque < itemPedido.Quantidade) 
            {
                throw new ItemPedidoExceptions("A quantidade solicitada do item em questao não se encontra disponível em estoque!!");
            }
            produto.Estoque -= itemPedido.Quantidade;
            itemPedido.PrecoUnitario = produto.Preco;

            db.ItemPedido.Add(itemPedido);
            await db.SaveChangesAsync();
            return TypedResults.Created();//$"/api/ItemPedido/{itemPedido.Id}",itemPedido
        })
        .WithName("CreateItemPedido")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            var affected = await db.ItemPedido
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteItemPedido")
        .WithOpenApi();
    }
}
