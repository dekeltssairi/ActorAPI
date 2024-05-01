using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ActorContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }

        public ActorContext(DbContextOptions<ActorContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the Actor entity
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Details);
                entity.Property(a => a.Type).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Rank).IsRequired();
                entity.Property(a => a.Source).IsRequired().HasMaxLength(50);

                entity.HasIndex(a => a.Rank).IsUnique();
            });
        }
    }
}
