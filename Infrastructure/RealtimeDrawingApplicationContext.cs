using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure
{
    public class RealtimeDrawingApplicationContext : DbContext
    {
        //private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RealtimeDrawingApplication";
        private readonly string _connectionString = "Data Source=C:\\Users\\user\\Desktop\\RealtimeDrawingApplication.db";

        public RealtimeDrawingApplicationContext()
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<DrawingCanvasObject> DrawingCanvasObjects { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuring User Domain class
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.UserSharedProjects);
            modelBuilder.Entity<User>().HasMany(u => u.UserCreatedProjects)
                .WithOne(cp => cp.ProjectCreator);

            //Configuring Project Domain class
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<Project>().HasMany(p => p.ProjectDrawingCanvasObjects);
            modelBuilder.Entity<Project>().HasMany(p => p.SharedUsers);

            //Configuring ProjectUser domain class
            modelBuilder.Entity<ProjectUser>().HasKey(pu => new { pu.ProjectId, pu.UserId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
