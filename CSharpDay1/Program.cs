// Console.WriteLine("Enter task name");



// var taskName = Console.ReadLine()?.Trim();



// if (string.IsNullOrWhiteSpace(taskName))
// {
//     Console.Error.WriteLine("Task can not be empty or whitespace.");
//     return;
    
// }

// Console.WriteLine("Task Name "+ taskName.ToUpper());

// Console.WriteLine("Character Count: " + taskName.Length);


// Console.WriteLine("First Character: " + taskName[0]);


// var workItem = new WorkItem();
// Console.WriteLine(workItem.ToString()); 
// class WorkItem
// {
//     public int Id { get; set;} = 101;
//     public string Title {get; set;} = "Learn C# Fundamentals";
//     public double EstimatesHours { get; set;} = 5.5;

//     public bool Completed {get; set;} = false;

//     public string Priority {get; set;} =    "High";

//     public DateTime Created {get; set;} = DateTime.Now;

//     public override string ToString()
//     {
//         return $"ID: {Id}\nTitle: {Title}\nEstimated Hours: {EstimatesHours}\nCompleted: {Completed}\nPriority: {Priority}\nCreated: {Created:dd-MMM-yyyy}";
//     }

// }




// Console.WriteLine("Enter priority (1-4):");

// var input =Console.ReadLine()?.Trim();

// if (string.IsNullOrWhiteSpace(input))
// {
//     Console.Error.WriteLine("Priority can not be empty or whitespace.");
//     return;
// }

// if(input == "1")
// {
//     Console.WriteLine("Critical");
// }
// else if(input == "2")
// {
//     Console.WriteLine("High");
// }
// else if(input == "3")
// {
//     Console.WriteLine("Medium");
// }
// else if(input == "4")
// {
//     Console.WriteLine("Low");
// }
// else
// {
//     Console.WriteLine("Invalid priority");
// }



// var d = input switch
// {
//     "1" => "Critical",
//     "2" => "High",
//     "3" => "Medium",
//     "4" => "Low",
//     _   => "Invalid priority"
// };


// Console.WriteLine(d);



var tasks = new List<string>();

for (int i=0; i<=5; i++)
{
    
    var taskName = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(taskName))
    {
        Console.Error.WriteLine("Task can not be empty or whitespace.");
        continue;
    }
    tasks.Add(taskName);



}

foreach (var task in tasks)
{
    Console.WriteLine($"{tasks.IndexOf(task)+1}. {task}");
}