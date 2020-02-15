using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class TransactionItemViewModel : TransactionViewModelBase
    {
        private readonly TransactionsViewModel _parent;

        public TransactionItemViewModel(TransactionsViewModel parentViewModel, ProductsTransaction transaction) : base(transaction)
        {
            _parent = parentViewModel;
        }

        public bool ShowDocumentLink { get => _parent.ShowDocumentLink; }
        public bool ShowNotes { get => _parent.ShowNotes; }
        public bool ShowType { get => _parent.ShowType; }
    }
}
