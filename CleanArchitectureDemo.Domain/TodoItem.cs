namespace CleanArchitectureDemo.Domain;

public class TodoItem : ITodoItem
{
    public TodoItem(string title)
    {
        this.Title = title;
        this.IsCompleted = false;
    }

    public string Title { get; set; }
    public bool IsCompleted { get; set; } = false;

    public void MarkAsCompleted()
    {
        this.IsCompleted = true;
    }
}