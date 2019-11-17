using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models
{
    public partial class TaskManagerContext : DbContext
    {
        public TaskManagerContext()
        {
        }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagAssignment> TagAssignments { get; set; }
        public virtual DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TagAssignment>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.TodoId })
                    .HasName("PK__TagsTodo__5C249BF9CFEEDFD8");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagAssignments)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__TagsTodos__TagId__5DCAEF64");

                entity.HasOne(d => d.Todo)
                    .WithMany(p => p.TagAssignments)
                    .HasForeignKey(d => d.TodoId)
                    .HasConstraintName("FK__TagsTodos__TodoI__5EBF139D");
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Todoes)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Todos__ProjectId__5FB337D6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
