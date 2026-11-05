using Microsoft.AspNetCore.Http.HttpResults;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Endpoints;

public static class TodoEndpoints
{
    // Extension method keeps Program.cs thin — a common minimal-API organisation pattern.
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/todos").WithTags("Todos");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetTodo");
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);

        return app;
    }

    // TypedResults make return types explicit, testable, and visible to OpenAPI.
    private static async Task<Ok<IReadOnlyList<Todo>>> GetAll(ITodoRepository repo, CancellationToken ct) =>
        TypedResults.Ok(await repo.GetAllAsync(ct));

    private static async Task<Results<Ok<Todo>, NotFound>> GetById(Guid id, ITodoRepository repo, CancellationToken ct) =>
        await repo.GetAsync(id, ct) is { } todo ? TypedResults.Ok(todo) : TypedResults.NotFound();

    private static async Task<Results<CreatedAtRoute<Todo>, ValidationProblem>> Create(
        CreateTodoRequest request, ITodoRepository repo, CancellationToken ct)
    {
        if (Validate(request.Title) is { } errors)
            return TypedResults.ValidationProblem(errors);

        var todo = await repo.AddAsync(request.Title, ct);
        return TypedResults.CreatedAtRoute(todo, "GetTodo", new { id = todo.Id });
    }

    private static async Task<Results<Ok<Todo>, NotFound, ValidationProblem>> Update(
        Guid id, UpdateTodoRequest request, ITodoRepository repo, CancellationToken ct)
    {
        if (Validate(request.Title) is { } errors)
            return TypedResults.ValidationProblem(errors);

        return await repo.UpdateAsync(id, request.Title, request.IsDone, ct) is { } todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, NotFound>> Delete(Guid id, ITodoRepository repo, CancellationToken ct) =>
        await repo.DeleteAsync(id, ct) ? TypedResults.NoContent() : TypedResults.NotFound();

    // Pattern matching for simple validation. For larger apps, consider FluentValidation.
    private static Dictionary<string, string[]>? Validate(string? title) => title switch
    {
        _ when string.IsNullOrWhiteSpace(title) => new() { ["title"] = ["Title is required."] },
        { Length: > 200 } => new() { ["title"] = ["Title must be 200 characters or fewer."] },
        _ => null
    };
}
