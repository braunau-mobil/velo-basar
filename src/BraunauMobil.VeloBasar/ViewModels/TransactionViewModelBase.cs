using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Pages;
using System;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionViewModelBase
    {
        public TransactionViewModelBase(ProductsTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            Transaction = transaction;
        }

        [Display(Name = "Summe")]
        [DataType(DataType.Currency)]
        public decimal Amount { get => Transaction.GetProductsSum(); }
        [Display(Name = "Artikelanzahl")]
        public int ProductCount { get => Transaction.Products.Count; }
        public ProductsTransaction Transaction { get; }

        public VeloPage GetDocumentPage()
        {
            return new VeloPage
            {
                Page = RoutingHelper.GetPageForModel<ShowFileModel>(),
                Parameter = new ShowFileParameter { FileId = Transaction.DocumentId.Value }
            };
        }
        public VeloPage GetSellerDetailsPage()
        {
            return new VeloPage
            {
                Page = RoutingHelper.GetPageForModel<Pages.Sellers.DetailsModel>(),
                Parameter = new Pages.Sellers.DetailsParameter { SellerId = Transaction.Seller.Id }
            };
        }
    }
}
