// src/TodoListApp/TodoList.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TodoListApp
{
    /// <summary>
    /// Класс, представляющий список задач.
    /// </summary>
    public class TodoList
    {
        private List<Task> tasks;
        private int nextId;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TodoList"/>.
        /// </summary>
        public TodoList()
        {
            tasks = new List<Task>();
            nextId = 1;
        }

        /// <summary>
        /// Добавляет новую задачу в список задач.
        /// </summary>
        /// <param name="task">Задача, которую нужно добавить.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если задача равна null.</exception>
        public void AddTask(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.Id = nextId++;
            tasks.Add(task);
        }

        /// <summary>
        /// Получает все задачи.
        /// </summary>
        /// <returns>Список всех задач.</returns>
        public IEnumerable<Task> GetAllTasks()
        {
            return tasks;
        }

        /// <summary>
        /// Получает задачи по статусу выполнения.
        /// </summary>
        /// <param name="isCompleted">Статус выполнения задач.</param>
        /// <returns>Список задач с указанным статусом выполнения.</returns>
        public IEnumerable<Task> GetTasksByStatus(bool isCompleted)
        {
            return tasks.Where(t => t.IsCompleted == isCompleted);
        }

        /// <summary>
        /// Отмечает задачу как выполненную.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи, которую нужно отметить как выполненную.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если задача с указанным идентификатором не найдена.</exception>
        public void MarkTaskAsCompleted(int taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new InvalidOperationException("Task not found.");
            }
            task.IsCompleted = true;
        }

        /// <summary>
        /// Удаляет задачу из списка задач.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи, которую нужно удалить.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если задача с указанным идентификатором не найдена.</exception>
        public void DeleteTask(int taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new InvalidOperationException("Task not found.");
            }
            tasks.Remove(task);
        }

        /// <summary>
        /// Сохраняет список задач в файл.
        /// </summary>
        /// <param name="filePath">Путь к файлу, в который нужно сохранить задачи.</param>
        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Id},{task.Title},{task.Description},{task.IsCompleted}");
                }
            }
        }

        /// <summary>
        /// Загружает список задач из файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу, из которого нужно загрузить задачи.</param>
        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4 && int.TryParse(parts[0], out int id) && bool.TryParse(parts[3], out bool isCompleted))
                    {
                        var task = new Task(parts[1], parts[2])
                        {
                            Id = id,
                            IsCompleted = isCompleted
                        };
                        tasks.Add(task);
                        nextId = Math.Max(nextId, id + 1);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Класс, представляющий задачу.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название задачи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание задачи.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Указывает, выполнена ли задача.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Task"/>.
        /// </summary>
        /// <param name="title">Название задачи.</param>
        /// <param name="description">Описание задачи.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если название задачи пустое или равно null.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если описание задачи равно null.</exception>
        public Task(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            Title = title;
            Description = description;
            IsCompleted = false;
        }

        /// <summary>
        /// Возвращает строковое представление задачи.
        /// </summary>
        /// <returns>Строковое представление задачи.</returns>
        public override string ToString()
        {
            return $"ID: {Id}, Title: {Title}, Description: {Description}, Completed: {IsCompleted}";
        }
    }
}
