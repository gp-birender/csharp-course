using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi.Models;

namespace TodoApi.Tests;

// Integration tests: spins up the real app in-memory (no network port) via WebApplicationFactory.
public sealed class TodoApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Root_ReturnsGreeting()
    {
        var text = await _client.GetStringAsync("/");
        Assert.Contains("Hello", text);
    }

    [Fact]
    public async Task Create_Then_Get_ReturnsTodo()
    {
        var create = await _client.PostAsJsonAsync("/todos", new CreateTodoRequest("Learn C#"));
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var created = await create.Content.ReadFromJsonAsync<Todo>();
        Assert.NotNull(created);

        var fetched = await _client.GetFromJsonAsync<Todo>($"/todos/{created.Id}");
        Assert.Equal("Learn C#", fetched!.Title);
        Assert.False(fetched.IsDone);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WithBlankTitle_ReturnsBadRequest(string title)
    {
        var response = await _client.PostAsJsonAsync("/todos", new CreateTodoRequest(title));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_And_Delete_Work()
    {
        var created = await (await _client.PostAsJsonAsync("/todos", new CreateTodoRequest("Ship it")))
            .Content.ReadFromJsonAsync<Todo>();

        var update = await _client.PutAsJsonAsync($"/todos/{created!.Id}", new UpdateTodoRequest("Ship it", true));
        var updated = await update.Content.ReadFromJsonAsync<Todo>();
        Assert.True(updated!.IsDone);

        var delete = await _client.DeleteAsync($"/todos/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        var missing = await _client.GetAsync($"/todos/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}
