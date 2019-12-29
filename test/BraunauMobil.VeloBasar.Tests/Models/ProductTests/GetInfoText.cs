using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using System;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class GetInfoText : TestBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GetInfoText()
        {
            _localizer = GetLocalizer();
        }

        [Fact]
        public void Empty()
        {
            var product = new Product();
            Assert.Equal("", product.GetInfoText(_localizer));
        }
        [Fact]
        public void AllDataSet()
        {
            var product = new Product
            {
                Brand = new Brand
                {
                    Name = "Marke"
                },
                Type = new ProductType
                {
                    Name = "Product Typ"
                },
                Description = "Beschreibung",
                FrameNumber = "Rahmennummer",
                Color = "Farbe"
            };
            Assert.Equal($"Marke - Product Typ{Environment.NewLine}Beschreibung Rahmennummer Farbe{Environment.NewLine}", product.GetInfoText(_localizer));
        }
    }
}
