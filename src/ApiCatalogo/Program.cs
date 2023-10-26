using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Repository;
using APICatalogo_essencial.Net6.Context;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

// Configure Services class
var builder = WebApplication.CreateBuilder(args);

// ignorando objetos quando um ciclo de referência é detectado durante a serialização
builder.Services.AddControllers()
                .AddJsonOptions(op => 
                    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title =  "APICatalogo", Version = "v1"});

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Autorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"Header de autorização JWT usando o esquema Bearer.\r\n\r\nInformate
        'Bearer'[espaço] e o seu token.\r\n\r\nExamplo:\'Bearer 12345abcdef\'",
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
            Array.Empty<string>()
        }
    });
});

var mappingConfig = new MapperConfiguration(mc => {
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppCatalogoContext>(op => op.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppCatalogoContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op => 
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
            ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])
            )
        }
    );

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure class
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
