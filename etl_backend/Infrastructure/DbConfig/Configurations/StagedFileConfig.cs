using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig.Configurations;

public class StagedFileConfig : IEntityTypeConfiguration<StagedFile>
{
    public void Configure(EntityTypeBuilder<StagedFile> builder)
    {
        builder.ToTable("staged_files");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.OriginalFileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.StoredFilePath)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(f => f.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(f => f.Stage)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(64);

        builder.Property(f => f.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(f => f.ErrorCode)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(64);

        // Timestamptz for UTC times in Postgres
        builder.Property(f => f.UploadedAt)
            .HasColumnType("timestamptz");

        builder.HasIndex(f => f.StoredFilePath).IsUnique(); // each saved file path is unique
        builder.HasIndex(f => new { f.Stage, f.Status });
        builder.HasIndex(f => f.UploadedAt);
        
        builder.HasOne(f => f.Schema)
            .WithOne()
            .HasForeignKey<StagedFile>(f => f.SchemaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}