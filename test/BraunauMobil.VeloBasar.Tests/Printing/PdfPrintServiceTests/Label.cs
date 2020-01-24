﻿using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Printing.PdfPrintServiceTests
{
    public class Label
    {
        [Fact]
        public void OneProduct()
        {
            var basar = new Basar
            {
                Date = new DateTime(2063, 04, 05),
                Id = 1,
                Name = "Fahrradbasar",
                Location = "Hopfenhausen",
                ProductCommission = 0.1m
            };
            var product = new Product
            {
                Brand = new Brand
                {
                    Name = "Marke"
                },
                Color = "Grün Blau Rot",
                Description = "Gepäcksträger, Licht, Korb",
                FrameNumber = "123-456-789",
                Id = 223,
                Price = 123.23m,
                TireSize = "38\"",
                Type = new ProductType
                {
                    Name = "Lastenrad"
                }
            };

            var factory = new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<SharedResource>(factory);

            var creator = new PdfPrintService(localizer);
            var doc = creator.CreateLabel(basar, product, new PrintSettings());
            Assert.NotNull(doc);
        }
    }
}