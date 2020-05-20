using BraunauMobil.VeloBasar.Models;
using System;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionDetailsViewModel : TransactionViewModelBase
    {
        public TransactionDetailsViewModel(ProductsTransaction transaction) : base(transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            Products = new ProductsViewModel(transaction.Products)
            {
                ShowFooter = true,
                FooterValue = Transaction.GetProductsSum(),
                ShowSeller = false
            };
        }

        public ProductsViewModel Products { get; }
    }
}
