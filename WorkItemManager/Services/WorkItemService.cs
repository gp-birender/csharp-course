using WorkItemManager.Models;

//namespace TaskSummaryApp.Models;

namespace WorkItemManager.Services;

public class WorkItemService
{
    private List<WorkItem> _items = new();
    private int _nextId = 1;

    public WorkItem Add(WorkItemDto itemDto )
    {
        if(string.IsNullOrWhiteSpace(itemDto.Title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }
        var item = new WorkItem
        {
            Id = _nextId++,
            Title = itemDto.Title,
            Priority = itemDto.Priority
        };

        _items.Add(item);
        return item;
    }
    public List<WorkItem> GetAll()
    {
        return _items;
    }
    public WorkItem? GetById(int id)
    {
        
          return _items.FirstOrDefault(item => item.Id == id);
    }

    public bool UpdateStatus(int id, WorkItemStatus newStatus)
    {
        var item = GetById(id);
        if (item == null)
        {
             return false;
        }
        item.Status = newStatus;
        return true;
    }
    public bool Delete(int id)
    {
        var item = GetById(id);
        if (item == null)
        {
            return false;
        }
        _items.Remove(item);
        return true;
    }
}