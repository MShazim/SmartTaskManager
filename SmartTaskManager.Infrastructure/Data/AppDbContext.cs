using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTaskManager.Core.Domain;

namespace SmartTaskManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        //public DbSet<TaskNote> TaskNotes { get; set; }
        //public DbSet<AuditLog> AuditLogs { get; set; }


        // Telling EF Core the key explicitly
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().HasKey(t => t.TaskId);
        }
    }
}
