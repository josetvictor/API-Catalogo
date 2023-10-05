using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinApiCatalogo.Context;
using MinApiCatalogo.Models;
using MinApiCatalogo.Services;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// https://macoratti.net/21/06/aspnc_tkjwtswag1.htm
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme.
                   \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.
                    \r\n\r\nExample: \'Bearer 12345abcdef\'",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

var cs = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

builder.Services.AddSingleton<ITokenService>(new TokenService());

// validação do token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
 
var app = builder.Build();

#region Rotas
app.MapGet("/", () => "Catalogo de produtos - 2022");

#region Login
// endpoint para login
app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
{
    if(userModel == null) return Results.BadRequest("Login Invalido");

    if (userModel.UserName == "jose" && userModel.Password == "numkey123")
    {
        var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"], userModel);
        return Results.Ok(new { token = tokenString });
    }
    else
        return Results.BadRequest("Login Inválido");
}).Produces(StatusCodes.Status400BadRequest)
  .Produces(StatusCodes.Status200OK)
  .WithName("Login")
  .WithTags("Autenticacao");
#endregion
// Rotas de categorias
#region categorias
app.MapPost("/categorias", async (AppDbContext db, Categoria categoria) => {
    if (categoria is null)
        return Results.NotFound();

    db.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
}).WithTags("Categorias");

// foi adicionado o require authorization para testar a proteção da API
app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

app.MapPut("/categorias/{id:int}", async (AppDbContext db, int id, Categoria categoria) =>
{
    if(categoria.CategoriaId != id) return Results.BadRequest();

    var categoriafinded = await db.Categorias.FindAsync(id);

    if (categoriafinded is null)
        return Results.NotFound();

    categoriafinded.Nome = categoria.Nome;
    categoriafinded.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok();
}).WithTags("Categorias");

app.MapDelete("/categorias/{id:int}", async (AppDbContext db, int id) =>
{
    var categoriafinded = await db.Categorias.FindAsync(id);

    if (categoriafinded is null)
        return Results.NotFound();

    db.Categorias.Remove(categoriafinded);
    await db.SaveChangesAsync();
    return Results.Ok();
}).WithTags("Categorias");
#endregion

// Rotas do produto
#region Produtos
app.MapPost("/produtos", async (AppDbContext db, Produto produto) => {
    if (produto is null)
        return Results.NotFound();

    db.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
}).WithTags("Produtos");

app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync()).WithTags("Produtos");

app.MapPut("/produtos/{id:int}", async (AppDbContext db, int id, Produto produto) =>
{
    if (produto.ProdutoId != id) return Results.BadRequest();

    var produtoDB = await db.Produtos.FindAsync(id);

    if (produtoDB is null)
        return Results.NotFound();

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao = produto.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok();
}).WithTags("Produtos");

app.MapDelete("/produtos/{id:int}", async (AppDbContext db, int id) =>
{
    var produtoDB = await db.Produtos.FindAsync(id);

    if (produtoDB is null)
        return Results.NotFound();

    db.Produtos.Remove(produtoDB);
    await db.SaveChangesAsync();
    return Results.Ok();
}).WithTags("Produtos");
#endregion

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Diferença de authentication e Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();