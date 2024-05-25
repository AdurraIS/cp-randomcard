using cp_randomcard.Entities;
using Microsoft.EntityFrameworkCore;

namespace cp_randomcard.Infra.Data;

public class CardContext : DbContext
{
    public DbSet<Card> Cards { get; set; }

    public CardContext(DbContextOptions<CardContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region CardTable
        modelBuilder.Entity<Card>().ToTable("CARD");

        modelBuilder.Entity<Card>(entity =>
        {
            entity.Property(e => e.Id)
                .HasColumnType("INTEGER")
                .UseIdentityColumn()
                .HasColumnName("CARD_ID");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR2(255)")
                .HasColumnName("CARD_TITLE");
            entity.Property(e => e.Atribute)
                .IsRequired()
                .HasColumnType("NVARCHAR2(255)")
                .HasColumnName("CARD_ATRIBUTE");
            entity.Property(e => e.Power)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("CARD_POWER");
            entity.Property(e => e.Health)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("CARD_HEALTH");
        });
        # endregion
    }
}
