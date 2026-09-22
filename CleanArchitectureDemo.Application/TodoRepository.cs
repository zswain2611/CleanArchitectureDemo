namespace CleanArchitectureDemo.Application;

using CleanArchitectureDemo.Domain;

public class TodoRepository : ITodoRepository
{
    private List<ITodoItem> _repository = new();

    public void CreateTodoItem(string title, bool isCompleted = false)
    {
        var todo = new TodoItem(title);
        todo.IsCompleted = isCompleted;
        _repository.Add(todo);
    }

    public IEnumerable<ITodoItem> ListTodos()
    {
        return _repository;
    }

    public void MarkTodoItemAsCompleted(int index)
    {
        if (index < 0 || index >= _repository.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        var todoItem = _repository[index];
        todoItem.MarkAsCompleted();
    }
}
