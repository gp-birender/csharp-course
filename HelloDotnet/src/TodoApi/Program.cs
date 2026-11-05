using TodoApi.Endpoints;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Dependency injection: register the store as a singleton (in-memory, shared across requests).
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddOpenApi();               // OpenAPI doc at /openapi/v1.json
builder.Services.AddProblemDetails();        // RFC 7807 error responses

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.MapGet("/", () => "Hello from .NET on Apple Silicon!");
app.MapTodoEndpoints();

app.Run();

// Exposes the implicit Program class to the test project (WebApplicationFactory<Program>).
public partial class Program;
