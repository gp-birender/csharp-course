namespace CollectionsLinq.Demos;

// Lambda expressions - small unnamed functions written inline:  (parameters) => body
// Call LambdaDemo.Run() from Program.cs to see every section's output.
public static class LambdaDemo
{
    public static void Run()
    {
        FromMethodToLambda();
        Shapes();
        LambdasWithCollections();
        Closures();
        ClosureGotcha();
    }

    // 1. A lambda is just a shorter way to write a method you pass around
    static void FromMethodToLambda()
    {
        Section("1. From method to lambda");
        List<int> numbers = [1, 2, 3, 4, 5, 6];

        var evens1 = numbers.FindAll(IsEven);                         // pass a named method
        var evens2 = numbers.FindAll(n => n % 2 == 0);               // same thing, inline lambda

        Console.WriteLine($"Named method: [{string.Join(", ", evens1)}]");
        Console.WriteLine($"Lambda:       [{string.Join(", ", evens2)}]");
    }

    static bool IsEven(int n) => n % 2 == 0;

    // 2. The different shapes a lambda can take
    static void Shapes()
    {
        Section("2. Lambda shapes");
        Func<int> noParams = () => 42;                                // no parameters
        Func<int, int> oneParam = x => x * x;                         // one parameter: () optional
        Func<int, int, int> twoParams = (a, b) => a + b;              // two or more: () required
        Func<int, string> withTypes = (int n) => $"#{n}";             // explicit parameter type
        Func<int, string> statementBody = n =>                        // block body: needs { } and return
        {
            if (n < 0) return "negative";
            return n == 0 ? "zero" : "positive";
        };
        var inferred = (string s) => s.ToUpper();                     // C# 10+: var works if types are given

        Console.WriteLine($"noParams() = {noParams()}");
        Console.WriteLine($"oneParam(5) = {oneParam(5)}");
        Console.WriteLine($"twoParams(2, 3) = {twoParams(2, 3)}");
        Console.WriteLine($"withTypes(7) = {withTypes(7)}");
        Console.WriteLine($"statementBody(-4) = {statementBody(-4)}");
        Console.WriteLine($"inferred(\"hi\") = {inferred("hi")}");
    }

    // 3. Where you'll use lambdas most: passing rules to collection methods
    static void LambdasWithCollections()
    {
        Section("3. Lambdas with collections");
        var tasks = SampleData.Tasks();

        var bensTask = tasks.Find(t => t.Owner == "Ben");
        tasks.Sort((a, b) => b.Priority.CompareTo(a.Priority));      // highest priority first
        int removed = tasks.RemoveAll(t => t.Done);

        Console.WriteLine($"First Ben task: {bensTask?.Title}");
        Console.WriteLine($"Removed {removed} done tasks");
        Console.WriteLine($"Sorted by priority: [{string.Join(", ", tasks.Select(t => $"{t.Title}(P{t.Priority})"))}]");
    }

    // 4. Closures: a lambda can use (capture) variables from outside it
    static void Closures()
    {
        Section("4. Closures");
        int minPriority = 2;
        Func<TaskItem, bool> isImportant = t => t.Priority >= minPriority;   // captures minPriority

        var tasks = SampleData.Tasks();
        Console.WriteLine($"minPriority = 2 -> {tasks.Count(isImportant)} important");

        minPriority = 3;                                              // lambda sees the CURRENT value
        Console.WriteLine($"minPriority = 3 -> {tasks.Count(isImportant)} important");

        // A captured variable lives as long as the lambda does
        Func<int> counter = MakeCounter();
        counter(); counter();
        Console.WriteLine($"Counter after 3 calls: {counter()}");
    }

    static Func<int> MakeCounter()
    {
        int count = 0;                                                // would normally die when method ends...
        return () => ++count;                                         // ...but the lambda keeps it alive
    }

    // 5. Gotcha: lambdas capture the VARIABLE, not its value at that moment
    static void ClosureGotcha()
    {
        Section("5. Closure gotcha");
        var actions = new List<Action>();
        int i = 0;
        while (i < 3)
        {
            actions.Add(() => Console.Write($"{i} "));               // all three share the same i
            i++;
        }
        Console.Write("Shared variable: ");
        foreach (var a in actions) a();                               // prints 3 3 3
        Console.WriteLine();

        actions.Clear();
        for (int j = 0; j < 3; j++)
        {
            int copy = j;                                             // fresh variable each loop
            actions.Add(() => Console.Write($"{copy} "));
        }
        Console.Write("Copy per loop:   ");
        foreach (var a in actions) a();                               // prints 0 1 2
        Console.WriteLine();

        // static lambdas cannot capture anything - the compiler stops accidental captures
        Func<int, int> doubler = static x => x * 2;
        Console.WriteLine($"static lambda doubler(4) = {doubler(4)}");
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");
}
