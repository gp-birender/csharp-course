namespace CollectionsLinq.Demos;

// Func / Action / Predicate - built-in delegate types: variables that hold a method.
//   Func<in1, ..., TResult>  returns a value (LAST type argument is the return type)
//   Action<in1, ...>         returns nothing (void)
//   Predicate<T>             takes T, returns bool (older; Func<T, bool> is used more)
// Call FuncActionDemo.Run() from Program.cs to see every section's output.
public static class FuncActionDemo
{
    public static void Run()
    {
        FuncBasics();
        ActionBasics();
        PredicateBasics();
        PassingBehaviour();
        ReturningFunctions();
        StoringInCollections();
    }

    // 1. Func: the last type is what it returns
    static void FuncBasics()
    {
        Section("1. Func");
        Func<int> getAnswer = () => 42;                       // no input, returns int
        Func<int, int> square = x => x * x;                    // int in, int out
        Func<string, int, string> repeat = (s, n) => string.Concat(Enumerable.Repeat(s, n));
        Func<int, int> squareFromMethod = Square;              // can also point at a named method

        Console.WriteLine($"getAnswer() = {getAnswer()}");
        Console.WriteLine($"square(6) = {square(6)}");
        Console.WriteLine($"repeat(\"ab\", 3) = {repeat("ab", 3)}");
        Console.WriteLine($"squareFromMethod(7) = {squareFromMethod(7)}");
    }

    static int Square(int x) => x * x;

    // 2. Action: does something, returns nothing
    static void ActionBasics()
    {
        Section("2. Action");
        Action sayHi = () => Console.WriteLine("Hi!");
        Action<string> greet = name => Console.WriteLine($"Hello, {name}");
        Action<string, int> log = (msg, level) => Console.WriteLine($"[L{level}] {msg}");

        sayHi();
        greet("Asha");
        log("Disk almost full", 2);

        // Multicast: += adds another method; calling runs them all in order
        Action<string> notify = s => Console.WriteLine($"  email: {s}");
        notify += s => Console.WriteLine($"  sms:   {s}");
        notify("Build passed");
    }

    // 3. Predicate<T> == Func<T, bool>. List<T> methods (Find, RemoveAll, Exists) use Predicate.
    static void PredicateBasics()
    {
        Section("3. Predicate");
        Predicate<int> isNegative = n => n < 0;
        Func<int, bool> isNegativeFunc = n => n < 0;          // same shape, different type

        List<int> numbers = [3, -1, 4, -5];
        Console.WriteLine($"Find(isNegative): {numbers.Find(isNegative)}");
        Console.WriteLine($"Where(isNegativeFunc): [{string.Join(", ", numbers.Where(isNegativeFunc))}]");
        // numbers.Where(isNegative) would NOT compile: Where wants Func<int, bool>, not Predicate<int>
    }

    // 4. The real power: a method that accepts behaviour as a parameter
    static void PassingBehaviour()
    {
        Section("4. Passing behaviour into a method");
        List<int> numbers = [1, 2, 3, 4];

        Console.WriteLine($"Transform double: [{string.Join(", ", Transform(numbers, n => n * 2))}]");
        Console.WriteLine($"Transform label:  [{string.Join(", ", Transform(numbers, n => $"#{n}"))}]");

        ForEachItem(numbers, n => Console.Write($"<{n}> "));
        Console.WriteLine();

        var result = Measure("Sum 1..1,000,000", () => Enumerable.Range(1, 1_000_000).Sum(x => (long)x));
        Console.WriteLine($"Result: {result}");
    }

    // A generic method + Func = a reusable "do this to every item" (this is basically LINQ's Select)
    static List<TOut> Transform<TIn, TOut>(List<TIn> items, Func<TIn, TOut> map)
    {
        var output = new List<TOut>();
        foreach (var item in items) output.Add(map(item));
        return output;
    }

    static void ForEachItem<T>(List<T> items, Action<T> action)
    {
        foreach (var item in items) action(item);
    }

    static T Measure<T>(string label, Func<T> work)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        T result = work();
        Console.WriteLine($"{label} took {sw.ElapsedMilliseconds} ms");
        return result;
    }

    // 5. A function that builds and returns another function
    static void ReturningFunctions()
    {
        Section("5. Returning functions");
        Func<decimal, decimal> addGst = MakeTaxCalculator(0.18m);
        Func<decimal, decimal> addVat = MakeTaxCalculator(0.20m);
        Console.WriteLine($"100 + GST = {addGst(100m)}, 100 + VAT = {addVat(100m)}");

        Func<int, int> addOne = x => x + 1;
        Func<int, int> doubleIt = x => x * 2;
        Func<int, int> addOneThenDouble = x => doubleIt(addOne(x));   // composing
        Console.WriteLine($"addOneThenDouble(5) = {addOneThenDouble(5)}");
    }

    static Func<decimal, decimal> MakeTaxCalculator(decimal rate) => amount => amount * (1 + rate);

    // 6. Store functions in a dictionary - a simple command table instead of a big switch
    static void StoringInCollections()
    {
        Section("6. Functions in a dictionary");
        var operations = new Dictionary<string, Func<double, double, double>>
        {
            ["+"] = (a, b) => a + b,
            ["-"] = (a, b) => a - b,
            ["*"] = (a, b) => a * b,
            ["/"] = (a, b) => a / b,
        };

        foreach (var op in operations.Keys)
        {
            Console.WriteLine($"8 {op} 2 = {operations[op](8, 2)}");
        }
    }

    static void Section(string title) => Console.WriteLine($"\n=== {title} ===");
}
