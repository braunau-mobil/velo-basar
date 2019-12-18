using BraunauMobil.VeloBasar.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace BraunauMobil.VeloBasar.Models
{
    public class ProductsTransaction : TransactionModel
    {
        public int? SellerId { get; set; }

        [Display(Name = "Verkäufer")]
        public Seller Seller { get; set; }

        [Display(Name = "Artikel")]
        public ICollection<ProductToTransaction> Products { get; set; }

        public decimal GetSum()
        {
            return Products.Sum(pt => pt.Product.AdjustPrice(Type, Basar));
        }
        public string GetSumText()
        {
            var sum = GetSum();
            return string.Format(CultureInfo.CurrentCulture, "{0:C}", sum);
        }
    }
}
