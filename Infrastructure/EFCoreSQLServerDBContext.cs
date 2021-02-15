using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure
{
    public class EFCoreSQLServerDBContext : DbContext
    {
        public EFCoreSQLServerDBContext() : base()
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuring User Model
            modelBuilder.Entity<User>().HasMany(u => u.Accounts)
                                       .WithOne(a => a.User)
                                       .HasForeignKey(a => a.User);

            //Configuring Account Model
            modelBuilder.Entity<Account>().HasMany(a => a.Projects)
                                          .WithOne(p => p.Account)
                                          .HasForeignKey(p => p.Account);
            modelBuilder.Entity<Account>().Property(a => a.User)
                .IsRequired();

            //Configuring Project Model
        }

    }
}
