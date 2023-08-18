﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TsdDelivery.Domain.Entities;

namespace TsdDelivery.Infrastructures.FluentAPIs;

public class ShippingRateConfig : IEntityTypeConfiguration<ShippingRate>
{
    public void Configure(EntityTypeBuilder<ShippingRate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Service)
            .WithMany(x => x.shippingRates)
            .HasForeignKey(x => x.ServiceId);

    }
}
