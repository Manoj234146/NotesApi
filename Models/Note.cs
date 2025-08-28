
using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models;

public class Note
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
