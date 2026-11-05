using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoRepository
{
    Task<IReadOnlyList<Todo>> GetAllAsync(CancellationToken ct = default);
    Task<Todo?> GetAsync(Guid id, CancellationToken ct = default);
    Task<Todo> AddAsync(string title, CancellationToken ct = default);
    Task<Todo?> UpdateAsync(Guid id, string title, bool isDone, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
