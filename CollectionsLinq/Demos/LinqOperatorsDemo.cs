namespace CollectionsLinq.Demos;

// LINQ operators - extension methods on IEnumerable<T> that filter, shape, sort,
// group and summarise data. They take lambdas (Func<...>) to describe the rule.
// Call LinqOperatorsDemo.Run() from Program.cs to see every section's output.
public static class LinqOperatorsDemo
{
    public static void Run()
    {
        Filtering();
        Projecting();
        Sorting();
        SingleElement();
        Quantifiers();
        Aggregates();
        Grouping();
        Joining();
        Paging();
        SetOperators();
        QuerySyntax();
    }

    // 1. Where - keep items that match
    static void Filtering()
    {
        Section("1. Filtering: Where, OfType");
        var tasks = SampleData.Tasks();
        var openHigh = tasks.Where(t => !t.Done && t.Priority == 3);
        Print("Open + priority 3", openHigh.Select(t => t.Title));

        var withIndex = tasks.Where((t, i) => i % 2 == 0);        // overload with index
        Print("Every other task", withIndex.Select(t => t.Title));

        object[] mixed = [1, "two", 3, "four", 5.0];
        Print("OfType<string>", mixed.OfType<string>());
    }

    // 2. Select - turn each item into something else
    static void Projecting()
    {
        Section("2. Projecting: Select, SelectMany");
        var tasks = SampleData.Tasks();
        Print("Titles", tasks.Select(t => t.Title));
        Print("Anonymous type", tasks.Take(2).Select(t => new { t.Id, Upper = t.Title.ToUpper() }));

        // SelectMany flattens a list of lists into one list
        List<string> sentences = ["fix the bug", "write the spec"];
        Print("Select (nested)", sentences.Select(s => $"[{string.Join("|", s.Split(' '))}]"));
        Print("SelectMany (flat)", sentences.SelectMany(s => s.Split(' ')));
    }

    // 3. OrderBy / ThenBy
    static void Sorting()
    {
        Section("3. Sorting: OrderBy, ThenBy");
        var tasks = SampleData.Tasks();
        var sorted = tasks
            .OrderByDescending(t => t.Priority)                    // first rule
            .ThenBy(t => t.Title);                                 // tie-breaker
        Print("Priority desc, then title", sorted.Select(t => $"P{t.Priority} {t.Title}"));

        Print("Order() on ints (.NET 7+)", new[] { 3, 1, 2 }.Order());
        // Unlike List.Sort(), these return a NEW sequence - the original list is unchanged.
    }

    // 4. Getting one item: First / Single / ElementAt (+ OrDefault versions)
    static void SingleElement()
    {
        Section("4. One element: First, Single, Last");
        var tasks = SampleData.Tasks();
        Console.WriteLine($"First(Owner == Ben): {tasks.First(t => t.Owner == "Ben").Title}");
        Console.WriteLine($"Last(): {tasks.Last().Title}");
        Console.WriteLine($"Single(Id == 4): {tasks.Single(t => t.Id == 4).Title}");
        Console.WriteLine($"FirstOrDefault(Owner == Zed): {tasks.FirstOrDefault(t => t.Owner == "Zed")?.Title ?? "null"}");

        try { tasks.First(t => t.Owner == "Zed"); }
        catch (InvalidOperationException) { Console.WriteLine("First(no match) -> InvalidOperationException"); }

        try { tasks.Single(t => t.Owner == "Ben"); }
        catch (InvalidOperationException) { Console.WriteLine("Single(2 matches) -> InvalidOperationException"); }
        // First = "give me the first, there may be many". Single = "there must be exactly one".
    }

    // 5. Any / All / Contains - yes/no questions
    static void Quantifiers()
    {
        Section("5. Yes/no: Any, All, Contains");
        var tasks = SampleData.Tasks();
        Console.WriteLine($"Any(): {tasks.Any()}");
        Console.WriteLine($"Any(Priority == 3): {tasks.Any(t => t.Priority == 3)}");
        Console.WriteLine($"All(Done): {tasks.All(t => t.Done)}");
        Console.WriteLine($"Select(Owner).Contains(\"Chen\"): {tasks.Select(t => t.Owner).Contains("Chen")}");
        // Prefer Any() over Count() > 0: Any stops at the first match.
    }

    // 6. Count / Sum / Min / Max / Average / MaxBy / Aggregate
    static void Aggregates()
    {
        Section("6. Aggregates");
        var tasks = SampleData.Tasks();
        Console.WriteLine($"Count(open): {tasks.Count(t => !t.Done)}");
        Console.WriteLine($"Sum(Priority): {tasks.Sum(t => t.Priority)}");
        Console.WriteLine($"Average(Priority): {tasks.Average(t => t.Priority):F2}");
        Console.WriteLine($"Min / Max(Priority): {tasks.Min(t => t.Priority)} / {tasks.Max(t => t.Priority)}");
        Console.WriteLine($"MaxBy(Priority): {tasks.MaxBy(t => t.Priority)?.Title}  (returns the item, not the number)");
        Console.WriteLine($"Aggregate (join owners): {tasks.Select(t => t.Owner).Distinct().Aggregate((acc, o) => acc + " & " + o)}");
    }

    // 7. GroupBy / CountBy / ToDictionary / ToLookup
    static void Grouping()
    {
        Section("7. Grouping: GroupBy, CountBy, ToDictionary");
        var tasks = SampleData.Tasks();

        foreach (var group in tasks.GroupBy(t => t.Owner))
        {
            Console.WriteLine($"{group.Key}: {group.Count()} tasks -> [{string.Join(", ", group.Select(t => t.Title))}]");
        }

        Print("CountBy(Owner) (.NET 9+)", tasks.CountBy(t => t.Owner).Select(p => $"{p.Key}={p.Value}"));

        Dictionary<int, string> byId = tasks.ToDictionary(t => t.Id, t => t.Title);
        Console.WriteLine($"ToDictionary -> byId[3] = {byId[3]}");
    }

    // 8. Join - combine two sequences on a matching key
    static void Joining()
    {
        Section("8. Joining: Join, Zip");
        var tasks = SampleData.Tasks();
        var owners = new[]
        {
            new { Name = "Asha", Team = "Platform" },
            new { Name = "Ben",  Team = "Payments" },
        };   // Chen has no team -> dropped by an inner join

        var joined = tasks.Join(owners,
            task => task.Owner,                                    // key from left
            owner => owner.Name,                                   // key from right
            (task, owner) => $"{task.Title} ({owner.Team})");      // result
        Print("Join", joined);

        string[] names = ["a", "b", "c"];
        int[] scores = [90, 75, 60];
        Print("Zip", names.Zip(scores, (n, s) => $"{n}:{s}"));
    }

    // 9. Skip / Take / Chunk - paging
    static void Paging()
    {
        Section("9. Paging: Skip, Take, Chunk");
        var numbers = Enumerable.Range(1, 10);
        int page = 2, pageSize = 3;
        Print($"Page {page} (size {pageSize})", numbers.Skip((page - 1) * pageSize).Take(pageSize));
        Print("TakeWhile(< 4)", numbers.TakeWhile(n => n < 4));
        Print("Chunk(4)", numbers.Chunk(4).Select(c => $"[{string.Join(",", c)}]"));
    }

    // 10. Distinct / Union / Intersect / Except (and the ...By versions)
    static void SetOperators()
    {
        Section("10. Set operators");
        int[] a = [1, 2, 2, 3, 4];
        int[] b = [3, 4, 5];
        Print("Distinct(a)", a.Distinct());
        Print("Union", a.Union(b));
        Print("Intersect", a.Intersect(b));
        Print("Except (a not in b)", a.Except(b));
        Print("DistinctBy(Owner)", SampleData.Tasks().DistinctBy(t => t.Owner).Select(t => $"{t.Owner}:{t.Title}"));
    }

    // 11. Query syntax - SQL-like keywords that compile to the SAME method calls
    static void QuerySyntax()
    {
        Section("11. Query syntax vs method syntax");
        var tasks = SampleData.Tasks();

        var query = from t in tasks
                    where !t.Done
                    orderby t.Priority descending
                    select t.Title;

        var method = tasks
            .Where(t => !t.Done)
            .OrderByDescending(t => t.Priority)
            .Select(t => t.Title);

        Print("Query syntax ", query);
        Print("Method syntax", method);
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");

    static void Print<T>(string label, IEnumerable<T> items) =>
        Console.WriteLine($"{label}: [{string.Join(", ", items)}]");
}
