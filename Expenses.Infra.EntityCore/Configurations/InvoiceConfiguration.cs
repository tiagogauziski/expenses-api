using Expenses.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Expenses.Infra.EntityCore.Configurations
{
    internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder
                .Property(e => e.Recurrence)
                .HasConversion(
                    v => JsonSerializer.Serialize<Recurrence>(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Recurrence>(v, new JsonSerializerOptions()));
        }
    }
}
