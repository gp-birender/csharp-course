using TaskSummary.Core;

var taskList = new TaskList();

if (args.Length > 0)
{
    // Stretch goal: read tasks from a file path given on the command line.
    string path = args[0];

    if (!File.Exists(path))
    {
        Console.Error.WriteLine($"File not found: {path}");
        return 1;
    }

    foreach (string line in File.ReadAllLines(path))
    {
        taskList.Add(line);
    }
}
else
{
    Console.WriteLine($"Enter tasks, one per line (max {TaskList.MaxLength} chars). Empty line to finish:");

    while (true)
    {
        string? line = Console.ReadLine();     // null = end of input (Ctrl+D or end of piped file)

        if (line is null || line.Length == 0)  // stop on end of input or a truly empty line
            break;

        taskList.Add(line);
    }
}

if (taskList.Tasks.Count == 0)
{
    Console.Error.WriteLine("No valid tasks entered.");
    return 1;
}

Console.WriteLine(taskList.BuildSummary());
return 0;
