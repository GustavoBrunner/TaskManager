using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Settings
{
    public class TaskManagerDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<TaskModel> Tasks { get; set; }
        
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) 
            : base(options) {   }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<ProjectModel>().ToTable("Project");
            b.Entity<TaskModel>().ToTable("Task");

        }
    }
}