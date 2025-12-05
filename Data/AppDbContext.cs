using Microsoft.EntityFrameworkCore;
using Sauvio.Models.User;

namespace Sauvio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.ConfirmationToken);
                entity.Property(e => e.IsConfirmed).HasDefaultValue(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.SourceOrCategory);
                entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

    }
}
