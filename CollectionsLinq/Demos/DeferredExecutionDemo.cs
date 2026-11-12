namespace CollectionsLinq.Demos;

// Deferred execution - most LINQ operators do NOT run when you write them.
// They build a "recipe" (IEnumerable<T>). The work happens only when you
// ENUMERATE it: foreach, ToList(), Count(), First(), Sum(), etc.
// Call DeferredExecutionDemo.Run() from Program.cs to see every section's output.
public static class DeferredExecutionDemo
{
    public static void Run()
    {
        NothingRunsYet();
        SeesLatestData();
        RunsEveryTime();
        LazyPipeline();
        DeferredVsImmediate();
        ExceptionsAreDeferred();
        WritingYourOwnWithYield();
    }

    // 1. Writing the query does nothing - watch when "checking" is printed
    static void NothingRunsYet()
    {
        Section("1. Nothing runs yet");
        List<int> numbers = [1, 2, 3];

        var query = numbers.Where(n =>
        {
            Console.WriteLine($"  checking {n}");
            return n > 1;
        });
        Console.WriteLine("Query created (no 'checking' lines above this one)");

        Console.WriteLine("Now enumerating with foreach:");
        foreach (var n in query)
        {
            Console.WriteLine($"  got {n}");
        }
    }

    // 2. The query reads the source when it runs, so it sees later changes
    static void SeesLatestData()
    {
        Section("2. Query sees the latest data");
        List<int> numbers = [1, 2, 3];
        var bigOnes = numbers.Where(n => n > 1);

        numbers.Add(10);                                    // added AFTER the query was written
        Console.WriteLine($"bigOnes: [{string.Join(", ", bigOnes)}]  (includes 10)");
    }

    // 3. Gotcha: every enumeration runs the whole query again
    static void RunsEveryTime()
    {
        Section("3. Runs again every time");
        int calls = 0;
        var query = Enumerable.Range(1, 5).Select(n => { calls++; return n * 10; });

        _ = query.Count();
        _ = query.Sum();
        _ = query.First();
        Console.WriteLine($"Count + Sum + First -> lambda ran {calls} times (5 + 5 + 1)");
        // If the lambda is expensive (or hits a database/API), this repeats the work.
        // Fix: materialise once with ToList() - see MaterialisationDemo.
    }

    // 4. Items flow through the pipeline one at a time, and stop early when they can
    static void LazyPipeline()
    {
        Section("4. Items flow one at a time");
        var result = Enumerable.Range(1, 1_000_000)
            .Where(n => { Console.WriteLine($"  Where  {n}"); return n % 2 == 0; })
            .Select(n => { Console.WriteLine($"  Select {n}"); return n * n; })
            .Take(2);

        Console.WriteLine($"Result: [{string.Join(", ", result)}]");
        Console.WriteLine("Only 4 of 1,000,000 items were ever looked at.");
    }

    // 5. Which operators are deferred and which run immediately?
    static void DeferredVsImmediate()
    {
        Section("5. Deferred vs immediate operators");
        Console.WriteLine("Deferred  (return IEnumerable<T>): Where, Select, SelectMany, OrderBy, GroupBy,");
        Console.WriteLine("                                   Skip, Take, Distinct, Concat, Zip, Chunk...");
        Console.WriteLine("Immediate (return a value/collection): ToList, ToArray, ToDictionary, ToHashSet,");
        Console.WriteLine("                                   Count, Sum, Min, Max, Average, Any, All,");
        Console.WriteLine("                                   First, Single, Last, ElementAt, Aggregate");

        List<int> numbers = [3, 1, 2];
        var deferred = numbers.Where(n => n > 0);           // IEnumerable<int> - recipe
        int immediate = numbers.Count(n => n > 0);          // int - already computed
        numbers.Add(5);
        Console.WriteLine($"After Add(5): deferred.Count() = {deferred.Count()}, immediate = {immediate}");
    }

    // 6. Errors inside a query also happen late - at enumeration, not where you wrote it
    static void ExceptionsAreDeferred()
    {
        Section("6. Exceptions are deferred too");
        List<int> numbers = [10, 5, 0];
        var divided = numbers.Select(n => 100 / n);         // no error here...
        Console.WriteLine("Query created fine");

        try
        {
            foreach (var d in divided) Console.WriteLine($"  {d}");
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("  ...DivideByZeroException thrown during foreach, not when the query was written");
        }
    }

    // 7. yield return - how deferred methods are built. Your own method can be lazy too.
    static void WritingYourOwnWithYield()
    {
        Section("7. Your own deferred method with yield");
        var evens = EvenNumbers().Take(3);                  // EvenNumbers is infinite - safe because it's lazy
        Console.WriteLine($"First 3 evens: [{string.Join(", ", evens)}]");
    }

    static IEnumerable<int> EvenNumbers()
    {
        int n = 0;
        while (true)
        {
            Console.WriteLine($"  producing {n}");
            yield return n;                                 // hand one value out, pause here until asked again
            n += 2;
        }
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");
}
