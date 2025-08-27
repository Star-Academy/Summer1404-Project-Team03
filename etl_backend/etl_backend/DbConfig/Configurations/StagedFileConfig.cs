using etl_backend.Domain;
using etl_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace etl_backend.DbConfig.Configurations;

public class StagedFileConfig : IEntityTypeConfiguration<StagedFile>
{
    public void Configure(EntityTypeBuilder<StagedFile> builder)
    {

        builder.HasKey(f => f.Id);

        builder.Property(f => f.OriginalFileName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.StoredFilePath)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(f => f.ErrorMessage)
            .HasMaxLength(1000);

        builder.Property(f => f.State)
            .IsRequired()
            .HasConversion<string>(); // store enum as string
        

        builder.HasOne(f => f.Schema)
            .WithOne()
            .HasForeignKey<StagedFile>(f => f.SchemaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}