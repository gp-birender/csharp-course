using System.Collections.Concurrent;
using TodoApi.Models;

namespace TodoApi.Services;

// Thread-safe in-memory store. Swap for an EF Core implementation later without touching endpoints.
public sealed class InMemoryTodoRepository(TimeProvider? clock = null) : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, Todo> _items = new();
    private readonly TimeProvider _clock = clock ?? TimeProvider.System;

    public Task<IReadOnlyList<Todo>> GetAllAsync(CancellationToken ct = default) =>
        Task.FromResult<IReadOnlyList<Todo>>(_items.Values.OrderBy(t => t.CreatedAt).ToList());

    public Task<Todo?> GetAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_items.GetValueOrDefault(id));

    public Task<Todo> AddAsync(string title, CancellationToken ct = default)
    {
        var todo = new Todo(Guid.NewGuid(), title.Trim(), false, _clock.GetUtcNow());
        _items[todo.Id] = todo;
        return Task.FromResult(todo);
    }

    public Task<Todo?> UpdateAsync(Guid id, string title, bool isDone, CancellationToken ct = default)
    {
        if (!_items.TryGetValue(id, out var existing))
            return Task.FromResult<Todo?>(null);

        var updated = existing with { Title = title.Trim(), IsDone = isDone };
        _items[id] = updated;
        return Task.FromResult<Todo?>(updated);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_items.TryRemove(id, out _));
}
