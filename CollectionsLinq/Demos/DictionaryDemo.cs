namespace CollectionsLinq.Demos;

// Dictionary<TKey, TValue> - key/value pairs with fast lookup by key.
// Keys are unique; order is not guaranteed.
// Call DictionaryDemo.Run() from Program.cs to see every section's output.
public static class DictionaryDemo
{
    public static void Run()
    {
        Creating();
        AddingAndReading();
        SafeReading();
        UpdatingAndRemoving();
        Looping();
        CountingWords();
        CaseInsensitiveKeys();
    }

    // 1. Three ways to create a dictionary
    static void Creating()
    {
        Section("1. Creating");
        var empty = new Dictionary<string, int>();         // empty
        var stock = new Dictionary<string, int>            // collection initializer
        {
            { "apple", 10 },
            { "banana", 5 }
        };
        var prices = new Dictionary<string, decimal>       // index initializer
        {
            ["apple"] = 0.50m,
            ["banana"] = 0.25m
        };

        Console.WriteLine($"empty.Count = {empty.Count}");
        Print("stock", stock);
        Print("prices", prices);
    }

    // 2. Add vs indexer, and reading by key
    static void AddingAndReading()
    {
        Section("2. Adding and reading");
        Dictionary<int, string> users = [];
        users.Add(1, "Asha");                              // Add: throws if key already exists
        users[2] = "Ben";                                  // indexer: adds, or overwrites if key exists
        users[2] = "Ben K.";                               // overwrite - no error
        Print("users", users);

        Console.WriteLine($"users[1] = {users[1]}");

        try
        {
            users.Add(1, "Duplicate");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Add duplicate key -> ArgumentException: {ex.Message}");
        }
    }

    // 3. Gotcha: reading a missing key throws. Use TryGetValue / ContainsKey / GetValueOrDefault.
    static void SafeReading()
    {
        Section("3. Safe reading");
        Dictionary<string, int> stock = new() { ["apple"] = 10 };

        try
        {
            Console.WriteLine(stock["mango"]);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"stock[\"mango\"] -> KeyNotFoundException: {ex.Message}");
        }

        if (stock.TryGetValue("apple", out int qty))       // best: one lookup, no exception
        {
            Console.WriteLine($"TryGetValue(\"apple\"): found {qty}");
        }

        Console.WriteLine($"ContainsKey(\"mango\"): {stock.ContainsKey("mango")}");
        Console.WriteLine($"GetValueOrDefault(\"mango\"): {stock.GetValueOrDefault("mango")}");
        Console.WriteLine($"GetValueOrDefault(\"mango\", -1): {stock.GetValueOrDefault("mango", -1)}");
    }

    // 4. Update a value, TryAdd, Remove
    static void UpdatingAndRemoving()
    {
        Section("4. Updating and removing");
        Dictionary<string, int> stock = new() { ["apple"] = 10, ["banana"] = 5 };

        stock["apple"] += 3;                               // read, change, write back
        bool added = stock.TryAdd("banana", 99);           // false: key exists, value unchanged
        bool removed = stock.Remove("banana");             // true if it was there
        Console.WriteLine($"TryAdd existing: {added}, Remove: {removed}");
        Print("stock", stock);
    }

    // 5. Looping over pairs, keys, values
    static void Looping()
    {
        Section("5. Looping");
        Dictionary<string, int> stock = new() { ["apple"] = 10, ["banana"] = 5, ["cherry"] = 0 };

        foreach (KeyValuePair<string, int> pair in stock)
        {
            Console.WriteLine($"{pair.Key} -> {pair.Value}");
        }

        foreach (var (fruit, count) in stock)              // deconstruct the pair
        {
            Console.WriteLine($"{fruit} has {count}");
        }

        Console.WriteLine($"Keys: [{string.Join(", ", stock.Keys)}]");
        Console.WriteLine($"Values: [{string.Join(", ", stock.Values)}]");
    }

    // 6. Classic use: counting things
    static void CountingWords()
    {
        Section("6. Counting words");
        string text = "the cat and the dog and the bird";
        Dictionary<string, int> counts = [];

        foreach (var word in text.Split(' '))
        {
            counts[word] = counts.GetValueOrDefault(word) + 1;
        }
        Print("counts", counts);
    }

    // 7. Keys compare exactly by default; pass a comparer to ignore case
    static void CaseInsensitiveKeys()
    {
        Section("7. Case-insensitive keys");
        var normal = new Dictionary<string, int> { ["Apple"] = 1 };
        var ignoreCase = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["Apple"] = 1 };

        Console.WriteLine($"normal.ContainsKey(\"apple\"): {normal.ContainsKey("apple")}");
        Console.WriteLine($"ignoreCase.ContainsKey(\"apple\"): {ignoreCase.ContainsKey("apple")}");
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");

    static void Print<TKey, TValue>(string label, Dictionary<TKey, TValue> dict) where TKey : notnull =>
        Console.WriteLine($"{label}: {{ {string.Join(", ", dict.Select(p => $"{p.Key}: {p.Value}"))} }}");
}
