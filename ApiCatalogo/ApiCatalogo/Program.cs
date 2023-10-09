using APICatalogo_essencial.Net6.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

// Configure Services class
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppCatalogoContext>(op =>
    op.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection))
    );

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
