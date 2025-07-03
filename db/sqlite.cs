using Microsoft.EntityFrameworkCore;
using hash_slinging_slasher.Models;

namespace hash_slinging_slasher.Database;

public class ImageSearchContext : DbContext
{
    public ImageSearchContext(DbContextOptions<ImageSearchContext> options) : base(options)
    {
    }

    public DbSet<ImageRecord> ImageRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the ImageRecord entity
        modelBuilder.Entity<ImageRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); //optional
            entity.Property(e => e.Hash).IsRequired();
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.OriginURL).HasMaxLength(500); //optional
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasIndex(e => e.Hash).IsUnique(); //index since we are searching by hash
        });

        base.OnModelCreating(modelBuilder);
    }
}