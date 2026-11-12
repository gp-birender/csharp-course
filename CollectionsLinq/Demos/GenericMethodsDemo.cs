namespace CollectionsLinq.Demos;

// Generic methods - write a method ONCE and use it with many types.
// <T> is a placeholder type; the compiler fills it in at each call.
// Call GenericMethodsDemo.Run() from Program.cs to see every section's output.
public static class GenericMethodsDemo
{
    public static void Run()
    {
        WhyGenerics();
        TypeInference();
        MultipleTypeParameters();
        Constraints();
        DefaultValues();
    }

    // 1. Without generics you'd need PrintInts, PrintStrings, PrintDoubles...
    static void WhyGenerics()
    {
        Section("1. Why generics");
        PrintAll<int>([1, 2, 3]);                          // T = int (written explicitly)
        PrintAll<string>(["a", "b"]);                      // T = string
    }

    static void PrintAll<T>(List<T> items) =>
        Console.WriteLine($"{typeof(T).Name}: [{string.Join(", ", items)}]");

    // 2. Usually you don't write <T> - the compiler works it out from the arguments
    static void TypeInference()
    {
        Section("2. Type inference");
        int a = 1, b = 2;
        Swap(ref a, ref b);                                // T inferred as int
        Console.WriteLine($"After Swap: a = {a}, b = {b}");

        string x = "left", y = "right";
        Swap(ref x, ref y);                                // T inferred as string
        Console.WriteLine($"After Swap: x = {x}, y = {y}");
    }

    static void Swap<T>(ref T first, ref T second) => (first, second) = (second, first);

    // 3. More than one type parameter
    static void MultipleTypeParameters()
    {
        Section("3. Multiple type parameters");
        var pair = MakePair("age", 30);                    // TFirst = string, TSecond = int
        Console.WriteLine($"Pair: {pair}");
    }

    static (TFirst, TSecond) MakePair<TFirst, TSecond>(TFirst first, TSecond second) => (first, second);

    // 4. Constraints ("where T : ...") let you use members of T safely
    static void Constraints()
    {
        Section("4. Constraints");
        Console.WriteLine($"Largest int: {Largest([4, 9, 2])}");
        Console.WriteLine($"Largest string: {Largest(["pear", "apple", "zebra"])}");
        // Largest([new object()]) would NOT compile: object is not IComparable<object>
    }

    // Without the constraint, item.CompareTo(...) would not compile - T could be anything.
    static T Largest<T>(List<T> items) where T : IComparable<T>
    {
        T max = items[0];
        foreach (var item in items)
        {
            if (item.CompareTo(max) > 0) max = item;
        }
        return max;
    }

    // Common constraints:
    //   where T : class          reference type
    //   where T : struct         value type
    //   where T : new()          has a parameterless constructor
    //   where T : notnull        cannot be null
    //   where T : SomeBaseClass  / ISomeInterface

    // 5. default(T): 0 for numbers, null for reference types, false for bool
    static void DefaultValues()
    {
        Section("5. default(T)");
        Console.WriteLine($"FirstOrFallback([]) for int:    {FirstOrFallback(new List<int>())}");
        Console.WriteLine($"FirstOrFallback([]) for string: {FirstOrFallback(new List<string>()) ?? "null"}");
        Console.WriteLine($"FirstOrFallback([7, 8]):        {FirstOrFallback(new List<int> { 7, 8 })}");
    }

    static T? FirstOrFallback<T>(List<T> items) => items.Count > 0 ? items[0] : default;

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");
}
