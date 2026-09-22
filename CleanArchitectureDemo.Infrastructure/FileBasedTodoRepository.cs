using System.Text.Json;
using CleanArchitectureDemo.Domain;

namespace CleanArchitectureDemo.Infrastructure;

public class FileBasedTodoRepository : ITodoRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _filePath;

    public FileBasedTodoRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "todos.txt");
    }

    public void CreateTodoItem(string title, bool isCompleted = false)
    {
        var todos = Load();
        var todo = new TodoItem(title);
        todo.IsCompleted = isCompleted;
        todos.Add(todo);
        Save(todos);
    }

    public IEnumerable<ITodoItem> ListTodos()
    {
        return Load();
    }

    public void MarkTodoItemAsCompleted(int index)
    {
        var todos = Load();

        if (index < 0 || index >= todos.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        todos[index].MarkAsCompleted();
        Save(todos);
    }

    private List<ITodoItem> Load()
    {
        if (!File.Exists(_filePath))
        {
            return new List<ITodoItem>();
        }

        var text = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(text))
        {
            return new List<ITodoItem>();
        }

        List<TodoFileEntry>? entries = null;
        try
        {
            entries = JsonSerializer.Deserialize<List<TodoFileEntry>>(text, JsonOptions);
        }
        catch (JsonException)
        {
            entries = null;
        }

        entries ??= new List<TodoFileEntry>();

        return entries
            .Select(entry =>
            {
                var todo = new TodoItem(entry.Title);
                todo.IsCompleted = entry.IsCompleted;
                return (ITodoItem)todo;
            })
            .ToList();
    }

    private void Save(List<ITodoItem> todos)
    {
        var entries = todos.Select(todo => new TodoFileEntry
        {
            Title = todo.Title,
            IsCompleted = todo.IsCompleted
        });

        var json = JsonSerializer.Serialize(entries, JsonOptions);
        File.WriteAllText(_filePath, json);
    }

    private sealed class TodoFileEntry
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
