using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Codeikoo.TodoApi.Data;
using Codeikoo.TodoApi.DTOs;
using Codeikoo.TodoApi.Models;

namespace Codeikoo.TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _db;

    public TodosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoItem>>> GetAll()
    {
        var items = await _db.Todos
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var item = await _db.Todos.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required");

        var item = new TodoItem { Title = dto.Title.Trim() };

        _db.Todos.Add(item);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TodoUpdateDto dto)
    {
        var item = await _db.Todos.FindAsync(id);
        if (item == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Title))
            item.Title = dto.Title.Trim();

        item.IsDone = dto.IsDone;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Todos.FindAsync(id);
        if (item == null) return NotFound();

        _db.Todos.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
