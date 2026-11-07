using WorkItemManager.Services;
using WorkItemManager.Models;

var service = new WorkItemService();

while (true)
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Add Work Item");
    Console.WriteLine("2. View All Work Items");
    Console.WriteLine("3. Update Work Item Status");
    Console.WriteLine("4. Delete Work Item");
    Console.WriteLine("5. Exit");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddWorkItem(service);
            break;
        case "2":
            ViewAllWorkItems(service);
            break;
        case "3":
            UpdateWorkItemStatus(service);
            break;
        case "4":
            DeleteWorkItem(service);
            break;
        case "5":
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

void AddWorkItem(WorkItemService service)
{
     Console.Write("Title: ");
    var title = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Title is required.");
        return;
    }

    Console.Write("Description: ");
    var description = Console.ReadLine() ?? string.Empty;

    Console.WriteLine("Priority: 1 = Low, 2 = Medium, 3 = High");
    Console.Write("Choose priority: ");
     var input = Console.ReadLine();

    Priority priority;
      switch (input)
    {
        case "1":
            priority = Priority.Low;
            break;

        case "2":
            priority = Priority.Medium;
            break;

        case "3":
            priority = Priority.High;
            break;

        default:
            Console.WriteLine("Invalid priority. Defaulting to Medium.");
            priority = Priority.Medium;
            break;
    }
      var dto = new WorkItemDto(
        title,
        description,
        priority
    );
    var item = service.Add(dto);

    Console.WriteLine($"Created work item #{item.Id}");
}

void ViewAllWorkItems( WorkItemService service)
{
    var items = service.GetAll();
    if (items.Count == 0)
    {
        Console.WriteLine("No work items found.");
        return;
    }

    foreach (var item in items)
    {
        Console.WriteLine($"#{item.Id} - {item.Title} - {item.Priority} - {item.Status}");
    }
}

void UpdateWorkItemStatus(WorkItemService service)
{
    Console.Write("Enter the ID of the work item to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    Console.WriteLine("New Status: 1 = Open, 2 = In Progress, 3 = Closed");
    Console.Write("Choose new status: ");
    var input = Console.ReadLine();

    WorkItemStatus newStatus;
    switch (input)
    {
        case "1":
            newStatus = WorkItemStatus.Todo;
            break;

        case "2":
            newStatus = WorkItemStatus.InProgress;
            break;

        case "3":
            newStatus = WorkItemStatus.Done;
            break;

        default:
            Console.WriteLine("Invalid status.");
            return;
    }

    if (service.UpdateStatus(id, newStatus))
    {
        Console.WriteLine($"Updated work item #{id} to {newStatus}");
    }
    else
    {
        Console.WriteLine($"Work item #{id} not found.");
    }
}

void DeleteWorkItem( WorkItemService service)
{
    Console.Write("Enter the ID of the work item to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    if (service.Delete(id))
    {
        Console.WriteLine($"Deleted work item #{id}");
    }
    else
    {
        Console.WriteLine($"Work item #{id} not found.");
    }
}

