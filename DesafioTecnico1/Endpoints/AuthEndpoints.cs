using DesafioTecnico1.DTOs;
using DesafioTecnico1.Model;
using DesafioTecnico1.Util;
using Microsoft.AspNetCore.Identity;

namespace DesafioTecnico1.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterDto dto, UserManager<IdentityUser> userManager) =>
        {
            var user = new IdentityUser { UserName = dto.UserName };
            var result = await userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Results.Ok("Usuário criado com sucesso!");
            }

            return Results.BadRequest(result.Errors);
        }).AllowAnonymous();

        group.MapPost("/login", async (LoginDto dto, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config) =>
        {
            var user = await userManager.FindByNameAsync(dto.UserName);
            if (user is null) return Results.BadRequest("Usuário ou senha inválidos.");

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Results.BadRequest("Usuário ou senha inválidos.");

            var token = JWTUtil.GenerateJwtToken(user, config);

            return Results.Ok(new { token });
        }).AllowAnonymous();
    }

}
