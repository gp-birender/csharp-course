namespace CollectionsLinq.Demos;

// Extension methods - add methods to a type you don't own (string, int, List<T>...)
// without changing it or inheriting from it. ALL of LINQ is built this way.
//
// Rules:
//   1. Put them in a static, non-generic, non-nested class.
//   2. The method is static, and the first parameter starts with "this".
//   3. The caller needs a "using" for the extension class's namespace.
// Call ExtensionMethodsDemo.Run() from Program.cs to see every section's output.
public static class ExtensionMethodsDemo
{
    public static void Run()
    {
        OnBuiltInTypes();
        WhatTheCompilerDoes();
        GenericExtensions();
        OnYourOwnTypes();
        Chaining();
        ExtensionMembersCSharp14();
    }

    // 1. Calling extension methods looks like calling normal methods
    static void OnBuiltInTypes()
    {
        Section("1. On built-in types");
        Console.WriteLine($"\"  \".IsBlank() = {"  ".IsBlank()}");
        Console.WriteLine($"\"Hello world\".Truncate(5) = {"Hello world".Truncate(5)}");
        Console.WriteLine($"7.IsBetween(1, 10) = {7.IsBetween(1, 10)}");
    }

    // 2. It's just syntax sugar for calling a static method
    static void WhatTheCompilerDoes()
    {
        Section("2. What the compiler does");
        string a = "Hello world".Truncate(5);                   // what you write
        string b = StringExtensions.Truncate("Hello world", 5); // what the compiler turns it into
        Console.WriteLine($"Extension syntax: {a}, static call: {b}");

        string? nothing = null;
        Console.WriteLine($"null.IsBlank() = {nothing.IsBlank()}  (no crash: it's a static call)");
    }

    // 3. Generic extension methods work on many types (exactly like LINQ)
    static void GenericExtensions()
    {
        Section("3. Generic extensions");
        List<int> numbers = [1, 2, 3, 4, 5, 6, 7];
        foreach (var chunk in numbers.InGroupsOf(3))
        {
            Console.WriteLine($"Group: [{string.Join(", ", chunk)}]");
        }
        Console.WriteLine($"numbers.IsNullOrEmpty() = {numbers.IsNullOrEmpty()}");
        Console.WriteLine($"empty list IsNullOrEmpty() = {new List<string>().IsNullOrEmpty()}");
    }

    // 4. On your own types - keep the record simple, put helpers in extensions
    static void OnYourOwnTypes()
    {
        Section("4. On your own types");
        var tasks = SampleData.Tasks();
        Console.WriteLine($"{tasks[1].Title}: {tasks[1].PriorityLabel()}");
        Console.WriteLine($"Open tasks: {tasks.OpenTasks().Count()}");
    }

    // 5. Extension methods that return something can be chained - this is how LINQ reads
    static void Chaining()
    {
        Section("5. Chaining");
        var result = "   The quick brown fox jumps   "
            .Trim()
            .Truncate(15)
            .ToUpper();
        Console.WriteLine($"Chained: '{result}'");
    }

    // 6. C# 14 (.NET 10): extension blocks also allow extension PROPERTIES
    static void ExtensionMembersCSharp14()
    {
        Section("6. C# 14 extension members");
        Console.WriteLine($"\"level\".IsPalindrome = {"level".IsPalindrome}");
        Console.WriteLine($"\"hello\".IsPalindrome = {"hello".IsPalindrome}");
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");
}

// ---- The extension classes ----

public static class StringExtensions
{
    // "this string text" means: this method appears on every string
    public static bool IsBlank(this string? text) => string.IsNullOrWhiteSpace(text);

    public static string Truncate(this string text, int maxLength) =>
        text.Length <= maxLength ? text : text[..maxLength] + "...";

    // C# 14 syntax: an extension block. Can hold methods AND properties.
    extension(string text)
    {
        public bool IsPalindrome => text.SequenceEqual(text.Reverse());
    }
}

public static class NumberExtensions
{
    public static bool IsBetween(this int value, int min, int max) => value >= min && value <= max;
}

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? items) => items is null || !items.Any();

    public static IEnumerable<List<T>> InGroupsOf<T>(this IEnumerable<T> items, int size)
    {
        var group = new List<T>(size);
        foreach (var item in items)
        {
            group.Add(item);
            if (group.Count == size)
            {
                yield return group;
                group = new List<T>(size);
            }
        }
        if (group.Count > 0) yield return group;
    }
}

public static class TaskItemExtensions
{
    public static string PriorityLabel(this TaskItem task) => task.Priority switch
    {
        3 => "High",
        2 => "Medium",
        _ => "Low"
    };

    public static IEnumerable<TaskItem> OpenTasks(this IEnumerable<TaskItem> tasks) => tasks.Where(t => !t.Done);
}
