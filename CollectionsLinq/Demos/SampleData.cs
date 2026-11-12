namespace CollectionsLinq.Demos;

// Shared sample data used by the lambda, LINQ, deferred-execution and materialisation demos.
public record TaskItem(int Id, string Title, string Owner, int Priority, bool Done);

public static class SampleData
{
    public static List<TaskItem> Tasks() =>
    [
        new(1, "Write spec",      "Asha", 2, true),
        new(2, "Fix login bug",   "Ben",  3, false),
        new(3, "Deploy to test",  "Asha", 1, false),
        new(4, "Review PR",       "Chen", 2, true),
        new(5, "Fix payment bug", "Ben",  3, false),
        new(6, "Update docs",     "Chen", 1, false),
    ];
}
