
using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

namespace NotesApi.Data;

public class NotesDb : DbContext
{
    public NotesDb(DbContextOptions<NotesDb> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
}
