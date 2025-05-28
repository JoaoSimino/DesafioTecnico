using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DesafioTecnico1.Data;
using DesafioTecnico1.Endpoints;
using DesafioTecnico1.Exceptions.Handlers;
using Microsoft.AspNetCore.Mvc;
using DesafioTecnico1.Event;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DesafioTecnicoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DesafioTecnicoContext") ?? throw new InvalidOperationException("Connection string 'DesafioTecnicoContext' not found.")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<ClienteExceptionHandler>();
builder.Services.AddExceptionHandler<ItemPedidoExceptionHandler>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PedidoCanceladoEvent>());


var app = builder.Build();

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapClienteEndpoints();

app.MapPedidoEndpoints();

app.MapProdutoEndpoints();

app.MapItemPedidoEndpoints();

app.Run();

public partial class Program { }