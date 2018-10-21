using IRunesWebApp.Models;
using Microsoft.EntityFrameworkCore;


namespace IRunesWebApp.Data
{
    public class IRunesDbContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TrackAlbum> TracksAlbums { get; set; }

        public DbSet<UserAlbum> UsersAlbums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder
                .UseSqlServer(@"Server=THINKPAD\SQLEXPRESS;Database=IRunes;Integrated Security=true")
                .UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TrackAlbum>().HasKey(x => new { x.AlbumId, x.TrackId });
            modelBuilder.Entity<UserAlbum>().HasKey(x => new { x.AlbumId, x.UserId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
