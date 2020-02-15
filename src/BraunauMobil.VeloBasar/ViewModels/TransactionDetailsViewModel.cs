using BraunauMobil.VeloBasar.Models;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionDetailsViewModel : TransactionViewModelBase
    {
        public TransactionDetailsViewModel(ProductsTransaction transaction) : base(transaction)
        {
            Contract.Requires(transaction != null);

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
