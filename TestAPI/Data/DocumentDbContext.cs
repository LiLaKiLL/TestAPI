using Microsoft.EntityFrameworkCore;
using TestAPI.Models;

namespace TestAPI.Data
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Document> Documents { get; set; }
    }
}
