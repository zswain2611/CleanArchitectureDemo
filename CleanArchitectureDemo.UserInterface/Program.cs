using CleanArchitectureDemo.Domain;
using CleanArchitectureDemo.Infrastructure;

ITodoRepository todoRepository = new FileBasedTodoRepository();
var isrunning = true;

while (isrunning)
{
    Console.WriteLine();
    Console.WriteLine("Todo Menu");
    Console.WriteLine("1. Add a to-do item");
    Console.WriteLine("2. List to-do items");
    Console.WriteLine("3. Mark a to-do item as complete");
    Console.WriteLine("4. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Enter to-do item: ");
            var title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Item cannot be empty.");
                break;
            }

            todoRepository.CreateTodoItem(title.Trim());
            Console.WriteLine("To-do added.");
            break;

        case "2":
            var todos = todoRepository.ListTodos().ToList();

            if (todos.Count == 0)
            {
                Console.WriteLine("No to-do items yet.");
                break;
            }

            Console.WriteLine("Your to-do items:");
            for (var i = 0; i < todos.Count; i++)
            {
                var status = todos[i].IsCompleted ? "Complete" : "Pending";
                Console.WriteLine($"{i + 1}. {todos[i].Title} ({status})");
            }
            break;

        case "3":
            var todoItems = todoRepository.ListTodos().ToList();

            if (todoItems.Count == 0)
            {
                Console.WriteLine("No to-do items yet.");
                break;
            }

            Console.WriteLine("Your to-do items:");
            for (var index = 0; index < todoItems.Count; index++)
            {
                var todo = todoItems[index];
                var status = todo.IsCompleted ? "completed" : "pending";
                Console.WriteLine($"{index + 1}. {todo.Title} ({status})");
            }

            Console.Write("Enter the number of the todo item to complete: ");
            var itemNumberInput = Console.ReadLine();

            if (int.TryParse(itemNumberInput, out var itemNumber) &&
                itemNumber >= 1 &&
                itemNumber <= todoItems.Count)
            {
                todoRepository.MarkTodoItemAsCompleted(itemNumber - 1);
                Console.WriteLine("Todo item marked as complete.");
            }
            else
            {
                Console.WriteLine("Invalid todo item number.");
            }
            break;

        case "4":
            Console.WriteLine("Goodbye.");
            isrunning = false;
            break;

        default:
            Console.WriteLine("Invalid option. Enter 1, 2, 3, or 4.");
            break;
    }
}
