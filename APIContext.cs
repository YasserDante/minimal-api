using Api;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace api
{
    public class APIContext:DbContext
    {
        public APIContext() : base()
        {
        }
        public DbSet<Symbol> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Symbol>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string server = "WIN-KF45KMECN9L";
            optionsBuilder.UseSqlServer(@"Server="+server+"\\;Database=DBMarket;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
        }
    }
}
