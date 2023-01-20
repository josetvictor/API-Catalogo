using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Bem-vindo a API Tarefas - {DateTime.Now}");

        app.MapGet("/tarefas", async(GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var tarefa = con.GetAll<Tarefa>().ToList();

            if (tarefa is null)
                return Results.NotFound();

            return Results.Ok(tarefa);
        });
        
        app.MapGet("/tarefas/{id}", async(GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var tarefa = con.Get<Tarefa>(id);

            if (tarefa is null)
                return Results.NotFound();

            return Results.Ok(tarefa);
        });

        app.MapPost("/tarefas", async (GetConnection cg, Tarefa tarefa) =>
        {
            using var con = await cg();
            var id = con.Insert(tarefa);
            return Results.Created($"/tarefa/{id}", tarefa);
        });

        app.MapPost("/tarefas", async (GetConnection cg, Tarefa tarefa) =>
        {
            using var con = await cg();
            var id = con.Update(tarefa);
            return Results.Ok();
        });

        app.MapDelete("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var deleted = con.Get<Tarefa>(id);

            if (deleted is null)
                return Results.NotFound();

            con.Delete(deleted);
            return Results.Ok(deleted);
        });

    }
}
