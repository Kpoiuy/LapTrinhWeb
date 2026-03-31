using BaiTap2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BaiTap2.Controllers;

public class TodoController : Controller
{
    private readonly string _todosFilePath;

    public TodoController(IWebHostEnvironment env)
    {
        _todosFilePath = Path.Combine(env.ContentRootPath, "Data", "todos.json");
        Directory.CreateDirectory(Path.GetDirectoryName(_todosFilePath)!);

        if (!System.IO.File.Exists(_todosFilePath))
        {
            var defaultTodos = new List<Todo>
            {
                new() { Id = "1", Title = "Đi chợ", Completed = false },
                new() { Id = "2", Title = "Chơi thể thao", Completed = false },
                new() { Id = "3", Title = "Chơi game", Completed = false },
                new() { Id = "4", Title = "Học bài", Completed = false }
            };
            SaveTodos(defaultTodos);
        }
    }

    public IActionResult Index()
    {
        return View(LoadTodos());
    }

    public IActionResult Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        var todo = LoadTodos().FirstOrDefault(t => t.Id == id);
        return todo == null ? NotFound() : View(todo);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Todo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Todo todo)
    {
        if (!ModelState.IsValid)
        {
            return View(todo);
        }

        var todos = LoadTodos();
        if (todos.Any(t => t.Id == todo.Id))
        {
            ModelState.AddModelError(nameof(Todo.Id), "Mã công việc đã tồn tại");
            return View(todo);
        }

        todos.Add(todo);
        SaveTodos(todos);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        var todo = LoadTodos().FirstOrDefault(t => t.Id == id);
        return todo == null ? NotFound() : View(todo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(string id, Todo todo)
    {
        if (id != todo.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(todo);
        }

        var todos = LoadTodos();
        var existingTodo = todos.FirstOrDefault(t => t.Id == id);
        if (existingTodo == null)
        {
            return NotFound();
        }

        existingTodo.Title = todo.Title;
        existingTodo.Completed = todo.Completed;
        SaveTodos(todos);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        var todo = LoadTodos().FirstOrDefault(t => t.Id == id);
        return todo == null ? NotFound() : View(todo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string id)
    {
        var todos = LoadTodos();
        var todo = todos.FirstOrDefault(t => t.Id == id);

        if (todo == null)
        {
            return NotFound();
        }

        todos.Remove(todo);
        SaveTodos(todos);
        return RedirectToAction(nameof(Index));
    }

    private List<Todo> LoadTodos()
    {
        if (!System.IO.File.Exists(_todosFilePath))
        {
            return new List<Todo>();
        }

        var json = System.IO.File.ReadAllText(_todosFilePath);
        return JsonSerializer.Deserialize<List<Todo>>(json) ?? new List<Todo>();
    }

    private void SaveTodos(List<Todo> todos)
    {
        var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(_todosFilePath, json);
    }
}
