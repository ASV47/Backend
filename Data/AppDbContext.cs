using Microsoft.EntityFrameworkCore;
using Codeikoo.TodoApi.Models;

namespace Codeikoo.TodoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
}
