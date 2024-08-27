using Azure;
using Microsoft.EntityFrameworkCore;
using talk2note.Domain.Entities;

namespace talk2note.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
     
    }
}
