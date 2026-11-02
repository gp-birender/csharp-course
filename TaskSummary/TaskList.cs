using System.Text;

namespace TaskSummary.Core;

// Outcome of trying to add one line. An enum makes every possible result explicit.
public enum AddResult
{
    Added,
    Duplicate,
    Blank,
    TooLong
}

// Holds the validated tasks and the counters. Contains no Console calls,
// so it can be unit-tested later (Day 9) without faking keyboard input.
public sealed class TaskList
{
    public const int MaxLength = 50;

    private readonly List<string> _tasks = new();


    private readonly HashSet<string> _seen = new(StringComparer.OrdinalIgnoreCase);

    // Read-only view: callers can read the list but cannot add to it directly.
    public IReadOnlyList<string> Tasks => _tasks;

    public int DuplicateCount { get; private set; }
    public int BlankCount { get; private set; }
    public int TooLongCount { get; private set; }

    public AddResult Add(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
        {
            BlankCount++;
            return AddResult.Blank;
        }

        string name = rawName.Trim();

        if (name.Length > MaxLength)
        {
            TooLongCount++;
            return AddResult.TooLong;
        }

        // HashSet.Add returns false when the item is already present.
        if (!_seen.Add(name))
        {
            DuplicateCount++;
            return AddResult.Duplicate;
        }

        _tasks.Add(name);
        return AddResult.Added;
    }

    public string BuildSummary()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < _tasks.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {_tasks[i]}");
        }

        sb.AppendLine("---");
        sb.Append($"{_tasks.Count} {Plural(_tasks.Count, "task", "tasks")}");

        if (DuplicateCount > 0)
            sb.Append($", {DuplicateCount} {Plural(DuplicateCount, "duplicate", "duplicates")} skipped");

        if (TooLongCount > 0)
            sb.Append($", {TooLongCount} rejected (too long)");

        if (BlankCount > 0)
            sb.Append($", {BlankCount} rejected (blank)");

        return sb.ToString();
    }

    private static string Plural(int count, string singular, string plural) =>
        count == 1 ? singular : plural;
}
