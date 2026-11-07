
namespace WorkItemManager.Models;

public class WorkItem
{
    public int Id{ get; set; }
    public string Title { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public string Description { get; set; } = string.Empty;
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Todo;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}