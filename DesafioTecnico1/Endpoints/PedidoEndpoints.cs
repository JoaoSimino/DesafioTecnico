using Microsoft.EntityFrameworkCore;
using DesafioTecnico1.Data;
using DesafioTecnico1.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using AutoMapper;
using DesafioTecnico1.DTOs;
using DesafioTecnico1.Exceptions;
using Microsoft.AspNetCore.Http;
using DesafioTecnico1.DTOsç;
using static DesafioTecnico1.Model.StatusPedido;
using DesafioTecnico1.Event;
using MediatR;
namespace DesafioTecnico1.Endpoints;

public static class PedidoEndpoints
{
    public static void MapPedidoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Pedido").WithTags(nameof(Pedido));

        group.MapGet("/", async (DesafioTecnicoContext db) =>
        {
            //return await db.Pedido.ToListAsync();
            var pedidos = await db.Pedido
                .Include(p => p.Itens) //carregando a lista de itens
                .ToListAsync();
            //criando objeto anonimo exatamente no formato necessario para evitar ficar criando muitos arquivos também de DTOs
            var pedidoDtos = pedidos.Select(p => new {
                Id = p.Id,
                ClientId = p.ClientId,
                DataPedido = p.DataPedido,
                Status = (int)p.Status,
                Itens = p.Itens.Select(i => new 
                {
                    Id = i.Id,
                    Quantidade = i.Quantidade,
                    Preco = i.PrecoUnitario
                }).ToList()
            }).ToList();
            return Results.Ok(pedidoDtos);

        })
        .WithName("GetAllPedidos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<IResult> (Guid id, DesafioTecnicoContext db) =>
        {
            //return await db.Pedido.AsNoTracking()
            //    .FirstOrDefaultAsync(model => model.Id == id)
            //    is Pedido model
            //        ? TypedResults.Ok(model)
            //        : TypedResults.NotFound();
            var pedido = await db.Pedido
                .Include(p => p.Itens)  // Inclui os Itens associados
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido is null) 
            {
                throw new ItemPedidoExceptions("Não foi encontrado o ID do Cliente enviado na base!!");
            }

            var resultado = new
            {
                pedido.Id,
                pedido.ClientId,
                pedido.DataPedido,
                Status = (int)pedido.Status,
                Itens = pedido.Itens.Select(i => new
                {
                    i.Id,
                    i.Quantidade,
                    i.PrecoUnitario
                })
            };
            return Results.Ok(resultado);

        })
        .WithName("GetPedidoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Pedido pedido, DesafioTecnicoContext db) =>
        {
            var affected = await db.Pedido
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    //.SetProperty(m => m.Id, pedido.Id)
                    .SetProperty(m => m.ClientId, pedido.ClientId)
                    .SetProperty(m => m.DataPedido, pedido.DataPedido)
                    .SetProperty(m => m.Status, pedido.Status)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePedido")
        .WithOpenApi();

        group.MapPost("/", async (PedidoDtoCadastro pedidoDto, DesafioTecnicoContext db, IMapper mapper) =>
        {
            var pedido = mapper.Map<Pedido>(pedidoDto);

            //validar se o clienteId existe e se nao throw Exception!
            if (db.Cliente.Find(pedido.ClientId) is null) 
            {
                throw new ClienteExceptions("Não foi encontrado cadastro para o Cliente solicitado!");
            }

            db.Pedido.Add(pedido);
            await db.SaveChangesAsync();
            return TypedResults.Created();//$"/api/Pedido/{pedido.Id}",pedido;
        })
        .WithName("CreatePedido")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            var affected = await db.Pedido
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePedido")
        .WithOpenApi();


        group.MapPatch("/{id}/status", async Task<IResult>(Guid id, PedidoAlterarStatusDto pedidoDto, DesafioTecnicoContext db, IMapper mapper, IMediator mediator) => 
        {
            var pedido = await db.Pedido
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (pedido is null)
            {
                throw new ItemPedidoExceptions("Pedido nao foi encontrado na base de dados!!");
            }

            if(pedidoDto.Status == 0) 
            {
                throw new ItemPedidoExceptions("Operacao nao permitida! Favor enviar 1 para Pago ou 2 para Cancelado!");
            }
            if (pedidoDto.Status is not (StatusPedidoEnum)(int)StatusPedidoEnum.Pago and
                    not (StatusPedidoEnum)(int)StatusPedidoEnum.Cancelado)
            {
                throw new ItemPedidoExceptions("Operação não permitida! Favor enviar 1 para Pago ou 2 para Cancelado!");
            }

            if (pedidoDto.Status is StatusPedidoEnum.Cancelado) 
            {
                await mediator.Publish(new PedidoCanceladoEvent(pedido.Id));
            }

            pedido.Status = pedidoDto.Status;
            
            await db.SaveChangesAsync();
            return Results.Ok();//pedido

        }).WithName("AlteraStatusPedido")
        .WithOpenApi();



        group.MapGet("/cliente/{id}", async Task<IResult> (Guid id, DesafioTecnicoContext db) =>
        {
            var pedidos = await db.Pedido
                .Where(p => p.ClientId == id)
                .Select(p => new
                {
                    id = p.Id,
                    clientId = p.ClientId,
                    dataPedido = p.DataPedido,
                    status = p.Status,
                    itens = p.Itens.Select(i => new
                    {
                        id = i.Id,
                        quantidade = i.Quantidade,
                        preco = i.PrecoUnitario
                    }).ToList()
                })
                .ToListAsync();

            return Results.Ok(pedidos);

        })
        .WithName("GetPedidosByClienteId")
        .WithOpenApi();
    }
}
