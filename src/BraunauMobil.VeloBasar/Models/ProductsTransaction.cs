using BraunauMobil.VeloBasar.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BraunauMobil.VeloBasar.Models
{
    public class ProductsTransaction : TransactionModel
    {
        public int? SellerId { get; set; }

        [Display(Name = "Verkäufer")]
        public Seller Seller { get; set; }

        [Display(Name = "Artikel")]
        [SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
        public ICollection<ProductToTransaction> Products { get; set; }

        public decimal GetSoldProductsSum()
        {
            return Products.GetSoldProducts().SumPrice();
        }
        public decimal GetSoldCommissionSum()
        {
            return Products.GetSoldProducts().Sum(p => p.GetCommisionAmount(Basar));
        }
        public decimal GetSoldTotal()
        {
            return Products.GetSoldProducts().Sum(p => p.GetCommissionedPrice(Basar));
        }
    }
}
