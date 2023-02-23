using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiTemplate.Api.Domain.Entities;
using WebApiTemplate.Api.Infrastructure.Constants;

namespace WebApiTemplate.Api.Infrastructure.EFC
{
    public class StoreEFC : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable(DbConstants.Tables.TableTodo, DbConstants.Schemas.SchemaTodo);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Body).HasMaxLength(1000).IsRequired(true);

            builder.Property(e => e.Title).HasMaxLength(200).IsRequired(true);
        }
    }
}
