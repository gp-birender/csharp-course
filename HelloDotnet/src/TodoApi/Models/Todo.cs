namespace TodoApi.Models;

// Records give you value equality, immutability and `with` expressions for free.
public sealed record Todo(Guid Id, string Title, bool IsDone, DateTimeOffset CreatedAt);

// Request DTOs — keep the API contract separate from the domain model.
public sealed record CreateTodoRequest(string Title);

public sealed record UpdateTodoRequest(string Title, bool IsDone);
