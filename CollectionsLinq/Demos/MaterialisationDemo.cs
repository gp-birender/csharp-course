namespace CollectionsLinq.Demos;

// Materialisation - running a deferred query NOW and storing the results
// in a real collection: ToList(), ToArray(), ToDictionary(), ToHashSet().
// After that, the results are a fixed snapshot that is cheap to reuse.
// Call MaterialisationDemo.Run() from Program.cs to see every section's output.
public static class MaterialisationDemo
{
    public static void Run()
    {
        SnapshotVsLive();
        RunOnceReuseMany();
        ToListVsToArray();
        OtherMaterialisers();
        SafeToModifySource();
        WhenNotToMaterialise();
    }

    // 1. ToList() freezes the results; the query stays "live"
    static void SnapshotVsLive()
    {
        Section("1. Snapshot vs live query");
        List<int> numbers = [1, 2, 3];
        IEnumerable<int> live = numbers.Where(n => n > 1);  // recipe
        List<int> snapshot = live.ToList();                  // runs now, stores [2, 3]

        numbers.Add(10);
        Console.WriteLine($"live:     [{string.Join(", ", live)}]  (re-runs, sees 10)");
        Console.WriteLine($"snapshot: [{string.Join(", ", snapshot)}]  (fixed)");
    }

    // 2. The main reason to materialise: do the expensive work once
    static void RunOnceReuseMany()
    {
        Section("2. Run once, reuse many times");
        int calls = 0;
        var query = SampleData.Tasks().Select(t => { calls++; return t.Title.ToUpper(); });

        _ = query.Count(); _ = query.First(); _ = query.Last();
        Console.WriteLine($"Without ToList: lambda ran {calls} times");

        calls = 0;
        var titles = query.ToList();                         // runs 6 times here...
        _ = titles.Count; _ = titles[0]; _ = titles[^1];     // ...and never again
        Console.WriteLine($"With ToList:    lambda ran {calls} times");
    }

    // 3. ToList vs ToArray - both materialise; pick by what you'll do next
    static void ToListVsToArray()
    {
        Section("3. ToList vs ToArray");
        var source = SampleData.Tasks().Where(t => !t.Done).Select(t => t.Id);

        List<int> list = source.ToList();
        int[] array = source.ToArray();

        list.Add(99);                                        // List can grow
        // array.Add(99);  -> does not compile: arrays are fixed size
        Console.WriteLine($"List (can Add):   [{string.Join(", ", list)}]");
        Console.WriteLine($"Array (fixed):    [{string.Join(", ", array)}]");
        Console.WriteLine("Use ToList when you'll add/remove; ToArray for a fixed, read-only-ish result.");
        // Collection expression spread also materialises:
        int[] spread = [.. source];
        Console.WriteLine($"[.. source]:      [{string.Join(", ", spread)}]");
    }

    // 4. Other ways to materialise into the shape you need
    static void OtherMaterialisers()
    {
        Section("4. ToDictionary, ToHashSet, ToLookup");
        var tasks = SampleData.Tasks();

        Dictionary<int, TaskItem> byId = tasks.ToDictionary(t => t.Id);
        HashSet<string> owners = tasks.Select(t => t.Owner).ToHashSet();
        ILookup<string, TaskItem> byOwner = tasks.ToLookup(t => t.Owner);   // like a Dictionary<key, list>

        Console.WriteLine($"byId[5].Title = {byId[5].Title}");
        Console.WriteLine($"owners = {{ {string.Join(", ", owners)} }}");
        Console.WriteLine($"byOwner[\"Ben\"] = [{string.Join(", ", byOwner["Ben"].Select(t => t.Title))}]");
        Console.WriteLine($"byOwner[\"Zed\"].Count() = {byOwner["Zed"].Count()}  (empty, no exception)");

        try { tasks.ToDictionary(t => t.Owner); }
        catch (ArgumentException) { Console.WriteLine("ToDictionary(Owner) -> ArgumentException: duplicate key 'Asha'"); }
    }

    // 5. Materialise to fix the "modified while enumerating" error
    static void SafeToModifySource()
    {
        Section("5. Modifying the source safely");
        var tasks = SampleData.Tasks();

        try
        {
            foreach (var t in tasks.Where(t => t.Done))     // deferred: still reading 'tasks'
            {
                tasks.Remove(t);
            }
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Deferred query + Remove -> InvalidOperationException");
        }

        tasks = SampleData.Tasks();
        foreach (var t in tasks.Where(t => t.Done).ToList()) // snapshot first, then change the source
        {
            tasks.Remove(t);
        }
        Console.WriteLine($"With ToList(): {tasks.Count} tasks left");
    }

    // 6. Don't call ToList() everywhere "just in case"
    static void WhenNotToMaterialise()
    {
        Section("6. When NOT to materialise");
        var numbers = Enumerable.Range(1, 1_000_000);

        // Wasteful: builds a 1,000,000-item list just to read the first match
        var wasteful = numbers.ToList().Where(n => n % 7 == 0).First();
        // Better: stays lazy and stops after 7 items
        var better = numbers.First(n => n % 7 == 0);
        Console.WriteLine($"Both give {wasteful} / {better}, but only one built a million-item list.");

        Console.WriteLine("Rules of thumb:");
        Console.WriteLine("  Materialise when: you'll enumerate more than once, the work is expensive,");
        Console.WriteLine("                    you need a snapshot, or you'll modify the source.");
        Console.WriteLine("  Stay lazy when:   you enumerate once, use First/Any/Take, or the data is huge.");
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");
}
