using Microsoft.EntityFrameworkCore;

namespace Homework12.Model.Data
{
    public sealed class ApplicationContext: DbContext
    {
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Client>? Clients { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Homework12;Trusted_Connection=True;");
        }
    }
}
