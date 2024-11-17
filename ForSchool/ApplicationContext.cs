using Azure.Core;
using ForSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Functionality> Functionalities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=School;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Attendance
            modelBuilder
                .Entity<Attendance>()
                .HasOne(e => e.Student)
                .WithMany(e => e.Attendances)
                .OnDelete(DeleteBehavior.NoAction);

            //Student
            modelBuilder
                .Entity<Student>()
                .HasOne(e => e.Class)
                .WithMany(e => e.Students)
                .OnDelete(DeleteBehavior.NoAction);

            //Functionality
            modelBuilder
                .Entity<Functionality>()
                .HasOne(e => e.Project)
                .WithMany(e => e.Functionalities)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Functionality>()
                .HasOne(e => e.Student)
                .WithMany(e => e.Functionalities)
                .OnDelete(DeleteBehavior.NoAction);

            //Teacher
            modelBuilder
                .Entity<Teacher>()
                .HasOne(e => e.Project)
                .WithMany(e => e.Teachers)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Teacher>()
                .HasOne(e => e.Class)
                .WithMany(e => e.Teachers)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
