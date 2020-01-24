using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Pages;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionViewModel
    {
        private readonly TransactionsViewModel _parent;

        public TransactionViewModel(TransactionsViewModel parentViewModel, ProductsTransaction transaction)
        {
            _parent = parentViewModel;
            Transaction = transaction;
        }

        [Display(Name = "Summe")]
        [DataType(DataType.Currency)]
        public decimal Amount { get => Transaction.GetProductsSum(); }
        [Display(Name = "Artikelanzahl")]
        public int ProductCount { get => Transaction.Products.Count; }
        public bool ShowDocumentLink { get => _parent.ShowDocumentLink; }
        public bool ShowNotes { get => _parent.ShowNotes; }
        public bool ShowType { get => _parent.ShowType; }
        public ProductsTransaction Transaction { get; }

        public VeloPage GetDocumentPage()
        {
            return new VeloPage
            {
                Page = RoutingHelper.GetPageForModel<ShowFileModel>(),
                Parameter = new ShowFileParameter { FileId = Transaction.DocumentId.Value }
            };
        }
    }
}
