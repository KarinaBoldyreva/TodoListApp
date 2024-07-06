// src/TodoListApp/Program.cs

using System;

namespace TodoListApp
{
    /// <summary>
    /// Главный класс приложения.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Главный метод, точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        static void Main(string[] args)
        {
            TodoList todoList = new TodoList();
            todoList.LoadFromFile("tasks.txt");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Todo List Application");
                Console.WriteLine("1. Add new task");
                Console.WriteLine("2. View all tasks");
                Console.WriteLine("3. Mark task as completed");
                Console.WriteLine("4. Delete a task");
                Console.WriteLine("5. Filter tasks by status");
                Console.WriteLine("6. Save and Exit");
                Console.Write("Select an option: ");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        AddNewTask(todoList);
                        break;
                    case "2":
                        ViewAllTasks(todoList);
                        break;
                    case "3":
                        MarkTaskAsCompleted(todoList);
                        break;
                    case "4":
                        DeleteTask(todoList);
                        break;
                    case "5":
                        FilterTasksByStatus(todoList);
                        break;
                    case "6":
                        todoList.SaveToFile("tasks.txt");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Добавляет новую задачу в список задач.
        /// </summary>
        /// <param name="todoList">Список задач.</param>
        static void AddNewTask(TodoList todoList)
        {
            Console.Write("Enter task title: ");
            var title = Console.ReadLine();
            Console.Write("Enter task description: ");
            var description = Console.ReadLine();

            try
            {
                todoList.AddTask(new Task(title, description));
                Console.WriteLine("Task added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding task: {ex.Message}");
            }
        }

        /// <summary>
        /// Отображает все задачи.
        /// </summary>
        /// <param name="todoList">Список задач.</param>
        static void ViewAllTasks(TodoList todoList)
        {
            Console.WriteLine("All Tasks:");
            foreach (var task in todoList.GetAllTasks())
            {
                Console.WriteLine(task);
            }
        }

        /// <summary>
        /// Отмечает задачу как выполненную.
        /// </summary>
        /// <param name="todoList">Список задач.</param>
        static void MarkTaskAsCompleted(TodoList todoList)
        {
            Console.Write("Enter task ID to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskId))
            {
                try
                {
                    todoList.MarkTaskAsCompleted(taskId);
                    Console.WriteLine("Task marked as completed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error marking task as completed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid task ID.");
            }
        }

        /// <summary>
        /// Удаляет задачу из списка задач.
        /// </summary>
        /// <param name="todoList">Список задач.</param>
        static void DeleteTask(TodoList todoList)
        {
            Console.Write("Enter task ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int taskId))
            {
                try
                {
                    todoList.DeleteTask(taskId);
                    Console.WriteLine("Task deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting task: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid task ID.");
            }
        }

        /// <summary>
        /// Фильтрует задачи по статусу выполнения.
        /// </summary>
        /// <param name="todoList">Список задач.</param>
        static void FilterTasksByStatus(TodoList todoList)
        {
            Console.WriteLine("Filter tasks by status:");
            Console.WriteLine("1. All tasks");
            Console.WriteLine("2. Completed tasks");
            Console.WriteLine("3. Incomplete tasks");
            Console.Write("Select an option: ");
            var option = Console.ReadLine();
            IEnumerable<Task> tasks = null;
            switch (option)
            {
                case "1":
                    tasks = todoList.GetAllTasks();
                    break;
                case "2":
                    tasks = todoList.GetTasksByStatus(true);
                    break;
                case "3":
                    tasks = todoList.GetTasksByStatus(false);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine(task);
                }
            }
        }
    }
}
