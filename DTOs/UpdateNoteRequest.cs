
using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTOs;

public class UpdateNoteRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }
}
