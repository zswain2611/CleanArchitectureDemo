namespace CleanArchitectureDemo.Domain;

public interface ITodoItem
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public void MarkAsCompleted();
}
