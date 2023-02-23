using consulteer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace consulteer.Infrastructure.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
    }
}
