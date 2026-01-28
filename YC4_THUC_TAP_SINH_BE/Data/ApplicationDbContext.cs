using Microsoft.EntityFrameworkCore;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<User_Role> UserRoles { get; set; }
        public DbSet<User_Function> UserFunctions { get; set; }
        public DbSet<Role_Function> RoleFunctions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==================== USER ====================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

                // Indexes
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.IsActive);
            });

            // ==================== ROLE ====================
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.RoleName).IsUnique();
            });

            // ==================== FUNCTION ====================
            modelBuilder.Entity<Function>(entity =>
            {
                entity.HasKey(e => e.FunctionId);
                entity.Property(e => e.FunctionCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FunctionName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.FunctionCode).IsUnique();
            });

            // ==================== USER_ROLE ====================
            modelBuilder.Entity<User_Role>(entity =>
            {
                // Composite Key
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.Property(e => e.AssignedAt).HasDefaultValueSql("GETDATE()");

                // Relationships
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RoleId);
            });

            // ==================== USER_FUNCTION ====================
            modelBuilder.Entity<User_Function>(entity =>
            {
                // Composite Key
                entity.HasKey(uf => new { uf.UserId, uf.FunctionId });

                entity.Property(e => e.AssignedAt).HasDefaultValueSql("GETDATE()");

                // Relationships
                entity.HasOne(uf => uf.User)
                    .WithMany(u => u.UserFunctions)
                    .HasForeignKey(uf => uf.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uf => uf.Function)
                    .WithMany(f => f.UserFunctions)
                    .HasForeignKey(uf => uf.FunctionId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.FunctionId);
            });

            // ==================== ROLE_FUNCTION ====================
            modelBuilder.Entity<Role_Function>(entity =>
            {
                // Composite Key
                entity.HasKey(rf => new { rf.RoleId, rf.FunctionId });

                entity.Property(e => e.AssignedAt).HasDefaultValueSql("GETDATE()");

                // Relationships
                entity.HasOne(rf => rf.Role)
                    .WithMany(r => r.RoleFunctions)
                    .HasForeignKey(rf => rf.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rf => rf.Function)
                    .WithMany(f => f.RoleFunctions)
                    .HasForeignKey(rf => rf.FunctionId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.RoleId);
                entity.HasIndex(e => e.FunctionId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
