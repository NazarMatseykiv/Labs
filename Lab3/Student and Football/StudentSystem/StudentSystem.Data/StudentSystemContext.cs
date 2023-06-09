﻿namespace P01_StudentSystem.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Configuration;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StudentConfiguration());

            builder.ApplyConfiguration(new CourseConfiguration());

            builder.ApplyConfiguration(new ResourcesConfiguration());

            builder.ApplyConfiguration(new HomeworkConfiguration());

            builder.ApplyConfiguration(new StudentCourseConfiguration());
        }
    }
}