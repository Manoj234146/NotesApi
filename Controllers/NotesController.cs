
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Models;
using NotesApi.DTOs;

namespace NotesApi.Controllers;

[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private readonly NotesDb _db;

    public NotesController(NotesDb db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request)
    {
        var note = new Note
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        var response = new NoteResponse
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt
        };

        return CreatedAtAction(nameof(GetById), new { id = note.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteResponse>>> GetAll([FromQuery] string? search = null)
    {
        IQueryable<Note> query = _db.Notes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var q = search.Trim();
            query = query.Where(n =>
                EF.Functions.Like(n.Title, $"%{q}%") ||
                (n.Content != null && EF.Functions.Like(n.Content, $"%{q}%"))
            );
        }

        var notes = await query
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NoteResponse
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return Ok(notes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var n = await _db.Notes.FindAsync(id);
        if (n == null)
        {
            return NotFound(new { message = "Note not found." });
        }

        var response = new NoteResponse
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedAt = n.CreatedAt
        };

        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNoteRequest request)
    {
        var n = await _db.Notes.FindAsync(id);
        if (n == null)
        {
            return NotFound(new { message = "Note not found." });
        }

        n.Title = request.Title;
        n.Content = request.Content;
        await _db.SaveChangesAsync();

        var response = new NoteResponse
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedAt = n.CreatedAt
        };

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var n = await _db.Notes.FindAsync(id);
        if (n == null)
        {
            return NotFound(new { message = "Note not found." });
        }

        _db.Notes.Remove(n);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Note deleted successfully." });
    }
}
