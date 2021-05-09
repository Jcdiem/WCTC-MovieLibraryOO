using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using MovieLibrary.DataModels.DB;

#nullable disable

namespace MovieLibrary
{
    public partial class dotnetfinalDbContext : DbContext
    {
        public dotnetfinalDbContext()
        {
        }

        public dotnetfinalDbContext(DbContextOptions<dotnetfinalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataModels.DB.Genre> Genres { get; set; }
        public virtual DbSet<DataModels.DB.Movie> Movies { get; set; }
        public virtual DbSet<DataModels.DB.MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<DataModels.DB.Occupation> Occupations { get; set; }
        public virtual DbSet<DataModels.DB.User> Users { get; set; }
        public virtual DbSet<DataModels.DB.UserMovie> UserMovies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
                optionsBuilder.UseSqlServer(@config["MovieContext:ConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.ReleaseDate).HasColumnName("ReleaseDate");
            });               

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.HasIndex(e => e.GenreId, "IX_MovieGenres_GenreId");

                entity.HasIndex(e => e.MovieId, "IX_MovieGenres_MovieId");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.GenreId);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.MovieId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.OccupationId, "IX_Users_OccupationId");

                entity.HasOne(d => d.Occupation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.OccupationId);
            });

            modelBuilder.Entity<UserMovie>(entity =>
            {
                entity.HasIndex(e => e.MovieId, "IX_UserMovies_MovieId");

                entity.HasIndex(e => e.UserId, "IX_UserMovies_UserId");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.UserMovies)
                    .HasForeignKey(d => d.MovieId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMovies)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
