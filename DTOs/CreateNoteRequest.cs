
using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTOs;

public class CreateNoteRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }
}

public class NoteResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
