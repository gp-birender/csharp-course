# HelloDotnet — sample .NET 10 solution

A small but production-shaped Minimal API with DI, records, TypedResults,
ProblemDetails validation errors, and xUnit unit + integration tests.

```
HelloDotnet.slnx
src/TodoApi/            Minimal API (Program.cs, Endpoints/, Models/, Services/)
tests/TodoApi.Tests/    xUnit tests (repository unit tests + WebApplicationFactory integration tests)
```

## Run

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/TodoApi        # http://localhost:5247
dotnet watch --project src/TodoApi      # hot reload while editing
```

## Try it

```bash
curl http://localhost:5247/
curl -X POST http://localhost:5247/todos -H 'Content-Type: application/json' -d '{"title":"Learn C#"}'
curl http://localhost:5247/todos
curl http://localhost:5247/openapi/v1.json
```
