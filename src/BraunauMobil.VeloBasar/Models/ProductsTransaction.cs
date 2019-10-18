using BraunauMobil.VeloBasar.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public IEnumerable<Product> GetProducts()
        {
            return Products.Select(pt => pt.Product);
        }
        public bool CanCancelNothing()
        {
            return !Products.All(p => p.Product.CanCancel());
        }
        public decimal GetSum()
        {
            return Products.Sum(pt => pt.Product.Price);
        }
    }
}
