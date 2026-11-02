
Console.Write("Enter numbers separated by spaces: ");
string? input = Console.ReadLine();          // string? = may be null (end of input)

if (string.IsNullOrWhiteSpace(input))
{
    Console.Error.WriteLine("Nothing entered.");
    return 1;                                // process exit code
}

var numbers = new List<int>();               // var => List<int>, fixed at compile time
var rejected = new List<string>();

foreach (var token in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
{
    if (int.TryParse(token, out int value))  // "Try" pattern: bool + out param, no exception
        numbers.Add(value);
    else
        rejected.Add(token);
}

if (numbers.Count == 0)
{
    Console.Error.WriteLine("No valid numbers.");
    return 1;
}

int total = 0;
for (int i = 0; i < numbers.Count; i++)
    total += numbers[i];

int intAverage = total / numbers.Count;               // integer division!
double average = (double)total / numbers.Count;

string size = average switch                           // switch expression (Day 2 preview)
{
    < 0  => "negative",
    < 10 => "small",
    _    => "large"
};

Console.WriteLine($"Count:   {numbers.Count}");
Console.WriteLine($"Total:   {total:N0}");
Console.WriteLine($"Average: {average:F2} ({size}), int average: {intAverage}");
if (rejected.Count > 0)
    Console.WriteLine($"Ignored: {string.Join(", ", rejected)}");

return 0;