using Microsoft.AspNetCore.Html;

using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public interface IVeloHtmlFactory
    : IBootstrapHtmlFactory
{
    IHtmlContent BaseDataStateLink<TEntity>(CrudItemModel<TEntity> item, ICrudRouter router)
        where TEntity : class, ICrudEntity, new();

    IHtmlContent ProductState(ProductEntity product);

    IHtmlContent ProductState(StorageState storageState, ValueState valueState);

    TableBuilder<TModel> Table<TModel>(IEnumerable<TModel> list);

    TableBuilder<ProductEntity> ProductsTable(IEnumerable<ProductEntity> products, bool showSum = false, bool showId = false, bool showState = false);

    TableBuilder<TItemViewModel> ProductsTable<TItemViewModel>(IEnumerable<TItemViewModel> products, Func<TItemViewModel, ProductEntity> getProduct, bool showSum = false, bool showId = false, bool showState = false);

    TableBuilder<SellerEntity> SellersTable(IEnumerable<SellerEntity> sellers);

    TableBuilder<TransactionEntity> TransactionsTable(IEnumerable<TransactionEntity> transactions, bool showType = false, bool showProducts = false);
}
