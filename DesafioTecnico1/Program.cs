using Microsoft.EntityFrameworkCore;
using DesafioTecnico1.Data;
using DesafioTecnico1.Endpoints;
using DesafioTecnico1.Exceptions.Handlers;
using Microsoft.AspNetCore.Mvc;
using DesafioTecnico1.Event;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DesafioTecnicoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DesafioTecnicoContext") ?? throw new InvalidOperationException("Connection string 'DesafioTecnicoContext' not found.")));


if (builder.Environment.IsEnvironment("Testing"))
{
    // não configure o middleware de autenticação aqui
}
else
{
    //-Adicionando JWT-//
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<DesafioTecnicoContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        // Fallback: todos os endpoints exigem autenticação
        options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
    //-Adicionando JWT-//

}


builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DesafioTecnico API", Version = "v1" });

    // Define o esquema de segurança
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira 'Bearer' seguido de espaço e o token JWT.\r\nExemplo: 'Bearer eyJhb...'"
    });

    // Aplica o esquema globalmente
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


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

if (!builder.Environment.IsEnvironment("Testing"))
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseHttpsRedirection();

app.MapClienteEndpoints();

app.MapPedidoEndpoints();

app.MapProdutoEndpoints();

app.MapItemPedidoEndpoints();

app.MapAuthEndpoints();

app.Run();

public partial class Program { }