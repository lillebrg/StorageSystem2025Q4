using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Storage> Storages { get; set; }

        public DbSet<Rack> Racks { get; set; }

        public DbSet<Shelf> Shelves { get; set; }

        public DbSet<BaseItem> BaseItems { get; set; }

        public DbSet<SpecificItem> SpecificItems { get; set; }

        public DbSet<BorrowRequest> BorrowRequests { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<NotificationSubscriptions> NotificationSubscriptions { get; set; }

        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<Storage>()
                .HasMany(u => u.Racks)
                .WithOne(r => r.Storage)
                .HasForeignKey(r => r.StorageId)
                .IsRequired();

            modelBuilder.Entity<Rack>()
                .HasMany(u => u.Shelves)
                .WithOne(r => r.Rack)
                .HasForeignKey(r => r.RackId)
                .IsRequired();

            modelBuilder.Entity<Shelf>()
                .HasMany(u => u.BaseItems)
                .WithOne(r => r.Shelf)
                .HasForeignKey(r => r.ShelfId);

            modelBuilder.Entity<BaseItem>()
                .HasMany(u => u.SpecificItems)
                .WithOne(r => r.BaseItem)
                .HasForeignKey(r => r.BaseItemId)
                .IsRequired();

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();
        }
    }
}
