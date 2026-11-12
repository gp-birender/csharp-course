namespace CollectionsLinq.Demos;

// List<T> - a resizable, ordered, index-based collection.
// Call ListDemo.Run() from Program.cs to see every section's output.
public static class ListDemo
{
    public static void Run()
    {
        Creating();
        AddingAndReading();
        SearchingAndRemoving();
        SortingAndLooping();
        CountVsCapacity();
        ModifyWhileLooping();
        ReferenceVsCopy();
    }

    // 1. Three ways to create a list
    static void Creating()
    {
        Section("1. Creating");
        var titles = new List<string>();                   // empty
        var numbers = new List<int> { 5, 3, 8 };           // collection initializer
        List<string> tags = ["backend", "urgent"];         // collection expression (C# 12)

        Console.WriteLine($"titles.Count = {titles.Count}");
        Print("numbers", numbers);
        Print("tags", tags);
    }

    // 2. Add, Insert, read and overwrite by index
    static void AddingAndReading()
    {
        Section("2. Adding and reading");
        List<string> titles = [];
        titles.Add("Write spec");                          // add to the end
        titles.Add("Fix bug");
        titles.AddRange(["Deploy", "Review"]);             // add many
        titles.Insert(0, "Plan");                          // add at an index (shifts the rest right)
        Print("After adds", titles);

        Console.WriteLine($"First: {titles[0]}, Last: {titles[^1]}");
        titles[1] = "Write spec v2";                       // overwrite in place
        Print("After titles[1] = ...", titles);
    }

    // 3. Contains, IndexOf, Find, Exists, Remove, RemoveAt, RemoveAll
    static void SearchingAndRemoving()
    {
        Section("3. Searching and removing");
        List<string> titles = ["Plan", "Write spec", "Fix bug", "Deploy", "Review"];

        Console.WriteLine($"Contains(\"Deploy\"): {titles.Contains("Deploy")}");
        Console.WriteLine($"IndexOf(\"Deploy\"): {titles.IndexOf("Deploy")}  (-1 if missing)");
        Console.WriteLine($"Find(starts with F): {titles.Find(t => t.StartsWith('F'))}");
        Console.WriteLine($"Exists(length > 10): {titles.Exists(t => t.Length > 10)}");

        titles.Remove("Review");                           // first match; returns bool
        titles.RemoveAt(0);                                // by index
        int removed = titles.RemoveAll(t => t.Contains("bug")); // by condition; returns count
        Print($"After removes ({removed} removed by RemoveAll)", titles);
    }

    // 4. Sort, Reverse, foreach
    static void SortingAndLooping()
    {
        Section("4. Sorting and looping");
        List<int> numbers = [5, 3, 8];
        numbers.Sort();
        Print("Sort()", numbers);
        numbers.Reverse();
        Print("Reverse()", numbers);

        foreach (var n in numbers)
        {
            Console.Write($"{n} ");
        }
        Console.WriteLine();
    }

    // 5. The list grows its internal array automatically (doubles when full)
    static void CountVsCapacity()
    {
        Section("5. Count vs Capacity");
        List<int> growing = [];
        for (int i = 0; i < 5; i++)
        {
            growing.Add(i);
            Console.WriteLine($"Count = {growing.Count}, Capacity = {growing.Capacity}");
        }
    }

    // 6. Gotcha: adding/removing inside foreach throws
    static void ModifyWhileLooping()
    {
        Section("6. Modify while looping");
        List<string> tags = ["backend", "urgent"];
        try
        {
            foreach (var t in tags)
            {
                tags.Add("new");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"InvalidOperationException: {ex.Message}");
        }

        // Fix: loop over a copy
        foreach (var t in tags.ToList())
        {
            tags.Add(t + "-copy");
        }
        Print("Fixed with ToList()", tags);
    }

    // 7. Gotcha: assigning a list does not copy it
    static void ReferenceVsCopy()
    {
        Section("7. Reference vs copy");
        List<string> tags = ["backend", "urgent"];
        var sameList = tags;
        var realCopy = new List<string>(tags);             // or tags.ToList()
        tags.Add("api");
        Console.WriteLine($"sameList.Count = {sameList.Count} (same list), realCopy.Count = {realCopy.Count} (separate list)");
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");

    static void Print<T>(string label, List<T> list) =>
        Console.WriteLine($"{label}: [{string.Join(", ", list)}]");
}
