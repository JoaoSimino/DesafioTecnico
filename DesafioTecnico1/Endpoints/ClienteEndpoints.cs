using Microsoft.EntityFrameworkCore;
using DesafioTecnico1.Data;
using DesafioTecnico1.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using DesafioTecnico1.DTOs;
using AutoMapper;
using Serilog;
namespace DesafioTecnico1.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cliente").WithTags(nameof(Cliente));

        group.MapGet("/", async (DesafioTecnicoContext db) =>
        {
            var listaDeClientes = await db.Cliente.ToListAsync();
            Log.Information($"Total de {listaDeClientes.Count}, numero de clientes retornados no Endpoint!!");
            return listaDeClientes;
        })
        .WithName("GetAllClientes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cliente>, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            return await db.Cliente.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Cliente model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClienteById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, ClienteDto clienteDto, DesafioTecnicoContext db, IMapper mapper) =>
        {
            var cliente = mapper.Map<Cliente>(clienteDto);

            var affected = await db.Cliente
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    //.SetProperty(m => m.Id, cliente.Id)
                    .SetProperty(m => m.Email, cliente.Email)
                    .SetProperty(m => m.Telefone, cliente.Telefone)
                    .SetProperty(m => m.DataCadastro, cliente.DataCadastro)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCliente")
        .WithOpenApi();

        group.MapPost("/", async (ClienteDto clienteDto, DesafioTecnicoContext db, IMapper mapper) =>
        {
            var cliente = mapper.Map<Cliente>(clienteDto);

            db.Cliente.Add(cliente);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cliente/{cliente.Id}",cliente);
        })
        .WithName("CreateCliente")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, DesafioTecnicoContext db) =>
        {
            var affected = await db.Cliente
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCliente")
        .WithOpenApi();
    }
}
