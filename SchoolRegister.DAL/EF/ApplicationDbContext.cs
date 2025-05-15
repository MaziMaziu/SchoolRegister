using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolRegister.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SchoolRegister.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        // table properties
        public DbSet<Grade> Grades { get; set; }
        public DbSet<SchoolRegister.Model.DataModels.Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectGroup> SubjectGroups { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //configuration commands
            optionsBuilder.UseLazyLoadingProxies(); //enable lazy loading proxies
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH - dziedziczenie użytkowników w jednej tabeli z dyskryminatorem UserType
            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers")
                .HasDiscriminator<int>("UserType")
                .HasValue<User>((int)RoleValue.User)
                .HasValue<Student>((int)RoleValue.Student)
                .HasValue<Parent>((int)RoleValue.Parent)
                .HasValue<Teacher>((int)RoleValue.Teacher);

            // Relacja User-Group (wiele użytkowników w jednej grupie)
            modelBuilder.Entity<User>()
                .HasOne<SchoolRegister.Model.DataModels.Group>(u => u.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            // Self-referencja User-Parent (jeden rodzic, wiele dzieci)
            modelBuilder.Entity<User>()
                .HasOne<Parent>(u => u.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(u => u.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacja Grade-Student
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Grade-Subject
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Subject-Teacher
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Teacher)
                .WithMany(t => t.Subjects)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tabela pośrednicząca SubjectGroup (wiele-do-wielu)
            modelBuilder.Entity<SubjectGroup>()
                .HasKey(sg => new { sg.SubjectId, sg.GroupId });

            modelBuilder.Entity<SubjectGroup>()
                .HasOne(sg => sg.Subject)
                .WithMany(s => s.SubjectGroups)
                .HasForeignKey(sg => sg.SubjectId);

            modelBuilder.Entity<SubjectGroup>()
                .HasOne(sg => sg.Group)
                .WithMany(g => g.SubjectGroups)
                .HasForeignKey(sg => sg.GroupId);
        }

    }
}
