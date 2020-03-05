using BraunauMobil.VeloBasar.Models.Base;
using System;
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

        public bool CanRevert()
        {
            return Type == TransactionType.Settlement;
        }
        public decimal GetProductsSum()
        {
            return Products.Select(pt => pt.Product).SumPrice();
        }
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
        public ChangeInfo CalculateChange(decimal amountGiven, IReadOnlyCollection<decimal> nominations)
        {
            switch (Type)
            {
                case TransactionType.Acceptance:
                case TransactionType.Lock:
                case TransactionType.MarkAsGone:
                case TransactionType.Release:
                    return new ChangeInfo();
                case TransactionType.Cancellation:
                    return new ChangeInfo(GetProductsSum(), nominations);
                case TransactionType.Settlement:
                    return new ChangeInfo(GetSoldProductsSum(), nominations);
                case TransactionType.Sale:
                    return CalculateSaleChange(amountGiven, nominations);
                default:
                    throw new InvalidOperationException();
            }
        }    
        private ChangeInfo CalculateSaleChange(decimal amountGiven, IReadOnlyCollection<decimal> nominations)
        {
            var sum = GetSoldProductsSum();
            if (amountGiven < sum)
            {
                return new ChangeInfo();
            }
            return new ChangeInfo(amountGiven - sum, nominations);
        }
    }
}
