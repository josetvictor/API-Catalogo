using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Repository;
using APICatalogo_essencial.Net6.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

// Configure Services class
var builder = WebApplication.CreateBuilder(args);

// ignorando objetos quando um ciclo de referência é detectado durante a serialização
builder.Services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mappingConfig = new MapperConfiguration(mc => {
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppCatalogoContext>(op =>
    op.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection))
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

app.UseAuthorization();

app.MapControllers();

app.Run();
