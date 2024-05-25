using cp_randomcard.Entities;
using Microsoft.EntityFrameworkCore;

namespace cp_randomcard.Infra.Data;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region UserTable
        modelBuilder.Entity<User>().ToTable("USER");

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .HasColumnType("INTEGER")
                .UseIdentityColumn()
                .HasColumnName("USER_ID");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasColumnType("NVARCHAR2(255)")
                .HasColumnName("USER_USERNAME");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnType("NVARCHAR2(255)")
                .HasColumnName("USER_PASSWORD");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnType("NUMBER(1)")
                .HasColumnName("USER_ISACTIVE");
        });
        #endregion
    }
}
