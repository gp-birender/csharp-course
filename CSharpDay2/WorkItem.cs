// namespace TaskSummaryApp.Models
// {
//     public class WorkItem
//     {
//         // Properties
//         public int Id { get; set; }
//         public string Title { get; set; }
//         public string Status { get; set; }
//         public string Priority { get; set; }

//         // Constructor
//         public WorkItem(int id, string title, string status, string priority)
//         {
//             Id = id;
//             Title = title;
//             Status = status;
//             Priority = priority;
//         }
//     }
// }


namespace TaskSummaryApp.Models;

public sealed record WorkItemDto(
    Guid Id,
    string Title,
    string Status
);

