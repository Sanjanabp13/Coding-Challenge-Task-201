using Microsoft.EntityFrameworkCore;
using CodingChallenge.Enums;
using ShopSiloApp.Enums;
namespace CodingChallenge.Models
{
    public class TaskDBcontext:DbContext
    {

        public TaskDBcontext(DbContextOptions<TaskDBcontext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(p => p.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v));
            modelBuilder.Entity<Tasks>()
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));
            modelBuilder.Entity<Tasks>()
             .Property(p => p.Priority)
             .HasConversion(
                 v => v.ToString(),
                 v => (Priority)Enum.Parse(typeof(Priority), v));

        

    }
         public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
   
    }
