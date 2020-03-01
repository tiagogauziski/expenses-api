using Expenses.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Expenses.Infrastructure.SqlServer.Configurations
{
    internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder
                .HasKey(k => k.Id);

            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(p => p.Name)
                .IsRequired(true);

            builder
                .Property(e => e.Recurrence)
                .HasConversion(
                    v => JsonSerializer.Serialize<Recurrence>(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Recurrence>(v, new JsonSerializerOptions()));
        }
    }
}
