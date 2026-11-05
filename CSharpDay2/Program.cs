//using TaskSummaryApp.Models;



// WorkItem item = new WorkItem(1, "Implement feature X", "In Progress", "High");  
// Console.WriteLine($"ID: {item.Id}");
// Console.WriteLine($"Title: {item.Title}");
// Console.WriteLine($"Status: {item.Status}");
// Console.WriteLine($"Priority: {item.Priority}");




Guid id = Guid.NewGuid();

var item1 = new WorkItemDto(
    id,
    "Learn C#",
    "Pending"
);

var item2 = new WorkItemDto(
    id,
    "Learn C#",
    "Pending"
);

// 2. Compare using ==
Console.WriteLine("Initial comparison:");
Console.WriteLine(item1 == item2); // True

// 3. Change item2 title using with expression
var item3 = item2 with
{
    Title = "Learn ASP.NET Core"
};

// 4. Compare again
Console.WriteLine("\nAfter changing title:");
Console.WriteLine(item1 == item3); // False

// Print all objects
Console.WriteLine("\nItem 1:");
Console.WriteLine(item1);

Console.WriteLine("\nItem 2:");
Console.WriteLine(item2);

Console.WriteLine("\nItem 3:");
Console.WriteLine(item3);

// 5. Try inheriting from sealed record
// Uncomment to test the compilation error.

// public record SpecialWorkItem : WorkItemDto
// {
// }

public sealed record WorkItemDto(
    Guid Id,
    string Title,
    string Status
);

