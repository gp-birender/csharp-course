using TodoApi.Services;

namespace TodoApi.Tests;

// Plain unit tests against the repository — no web host involved.
public sealed class InMemoryTodoRepositoryTests
{
    private readonly InMemoryTodoRepository _repo = new();

    [Fact]
    public async Task Add_TrimsTitle_And_DefaultsToNotDone()
    {
        var todo = await _repo.AddAsync("  Write tests  ");

        Assert.Equal("Write tests", todo.Title);
        Assert.False(todo.IsDone);
    }

    [Fact]
    public async Task Update_UnknownId_ReturnsNull()
    {
        Assert.Null(await _repo.UpdateAsync(Guid.NewGuid(), "x", true));
    }

    [Fact]
    public async Task Delete_RemovesItem()
    {
        var todo = await _repo.AddAsync("Temp");

        Assert.True(await _repo.DeleteAsync(todo.Id));
        Assert.Empty(await _repo.GetAllAsync());
    }
}
