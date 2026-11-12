namespace CollectionsLinq.Demos;

// HashSet<T> - a collection of UNIQUE items with very fast Contains.
// No indexes, no duplicates, order is not guaranteed.
// Call HashSetDemo.Run() from Program.cs to see every section's output.
public static class HashSetDemo
{
    public static void Run()
    {
        Creating();
        AddingAndChecking();
        RemoveDuplicates();
        SetOperations();
        SetComparisons();
        CaseInsensitive();
        ListVsHashSetLookup();
    }

    // 1. Creating - duplicates are dropped automatically
    static void Creating()
    {
        Section("1. Creating");
        var empty = new HashSet<string>();
        var numbers = new HashSet<int> { 1, 2, 2, 3, 3, 3 };
        HashSet<string> tags = ["backend", "urgent", "backend"];

        Console.WriteLine($"empty.Count = {empty.Count}");
        Print("numbers (from 1,2,2,3,3,3)", numbers);
        Print("tags", tags);
    }

    // 2. Add returns bool - false means "already there"
    static void AddingAndChecking()
    {
        Section("2. Adding and checking");
        HashSet<string> seen = [];

        Console.WriteLine($"Add(\"a\"): {seen.Add("a")}");
        Console.WriteLine($"Add(\"b\"): {seen.Add("b")}");
        Console.WriteLine($"Add(\"a\") again: {seen.Add("a")}  (duplicate ignored)");
        Console.WriteLine($"Contains(\"b\"): {seen.Contains("b")}");
        Console.WriteLine($"Remove(\"b\"): {seen.Remove("b")}");
        Print("seen", seen);

        // No index: seen[0] does not compile. Loop with foreach instead.
        foreach (var item in seen)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
    }

    // 3. Classic use: de-duplicate a list
    static void RemoveDuplicates()
    {
        Section("3. Removing duplicates");
        List<string> emails = ["a@x.com", "b@x.com", "a@x.com", "c@x.com", "b@x.com"];
        var unique = new HashSet<string>(emails);
        Console.WriteLine($"List had {emails.Count}, set has {unique.Count}");
        Print("unique", unique);

        // Find the duplicates: Add returns false the second time
        HashSet<string> seen = [];
        var duplicates = emails.Where(e => !seen.Add(e)).ToList();
        Console.WriteLine($"Duplicates: [{string.Join(", ", duplicates)}]");
    }

    // 4. Set math. These METHODS CHANGE the set they are called on, so work on copies.
    static void SetOperations()
    {
        Section("4. Set operations");
        HashSet<string> alice = ["csharp", "sql", "docker"];
        HashSet<string> bob = ["csharp", "python", "docker", "aws"];

        var union = new HashSet<string>(alice);
        union.UnionWith(bob);                              // in either
        Print("UnionWith (either)", union);

        var both = new HashSet<string>(alice);
        both.IntersectWith(bob);                           // in both
        Print("IntersectWith (both)", both);

        var onlyAlice = new HashSet<string>(alice);
        onlyAlice.ExceptWith(bob);                         // in alice but not bob
        Print("ExceptWith (only alice)", onlyAlice);

        var notShared = new HashSet<string>(alice);
        notShared.SymmetricExceptWith(bob);                // in one but not both
        Print("SymmetricExceptWith (not shared)", notShared);

        Print("alice is unchanged", alice);
    }

    // 5. Comparing sets - these return bool and don't change anything
    static void SetComparisons()
    {
        Section("5. Set comparisons");
        HashSet<string> required = ["csharp", "sql"];
        HashSet<string> candidate = ["csharp", "sql", "docker"];

        Console.WriteLine($"required.IsSubsetOf(candidate): {required.IsSubsetOf(candidate)}");
        Console.WriteLine($"candidate.IsSupersetOf(required): {candidate.IsSupersetOf(required)}");
        Console.WriteLine($"required.Overlaps([\"sql\", \"go\"]): {required.Overlaps(["sql", "go"])}");
        Console.WriteLine($"SetEquals (order ignored): {required.SetEquals(["sql", "csharp"])}");
    }

    // 6. Same as Dictionary: pass a comparer to ignore case
    static void CaseInsensitive()
    {
        Section("6. Case-insensitive");
        HashSet<string> normal = ["Admin", "admin", "ADMIN"];
        var ignoreCase = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Admin", "admin", "ADMIN" };

        Print("normal", normal);
        Print("ignoreCase", ignoreCase);
    }

    // 7. Why use a HashSet: Contains is O(1) vs List's O(n)
    static void ListVsHashSetLookup()
    {
        Section("7. List vs HashSet lookup speed");
        const int size = 100_000;
        const int lookups = 10_000;
        List<int> list = Enumerable.Range(0, size).ToList();
        HashSet<int> set = [.. list];

        var sw = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < lookups; i++) list.Contains(size - 1);   // scans to the end each time
        long listMs = sw.ElapsedMilliseconds;

        sw.Restart();
        for (int i = 0; i < lookups; i++) set.Contains(size - 1);    // jumps straight to it
        long setMs = sw.ElapsedMilliseconds;

        Console.WriteLine($"{lookups:N0} lookups in {size:N0} items -> List: {listMs} ms, HashSet: {setMs} ms");
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");

    static void Print<T>(string label, IEnumerable<T> items) =>
        Console.WriteLine($"{label}: {{ {string.Join(", ", items)} }}");
}
