﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public interface IVeloHtmlFactory
    : IBootstrapHtmlFactory
{
    TagBuilder Alert(MessageType type, string text);

    TagBuilder Alert(MessageType type, string title, string text);

    TagBuilder Badge(BadgeType type);

    IHtmlContent ProductState(ProductEntity product);

    IHtmlContent ProductState(StorageState storageState, ValueState valueState, bool donateIfNotSold);

    TableBuilder<ProductEntity> ProductsTable(IEnumerable<ProductEntity> products, bool showSum = false, bool showId = false, bool showState = false);

    TableBuilder<TItemViewModel> ProductsTable<TItemViewModel>(IEnumerable<TItemViewModel> products, Func<TItemViewModel, ProductEntity> getProduct, bool showSum = false, bool showId = false, bool showState = false);

    TableBuilder<SellerEntity> SellersTable(IEnumerable<SellerEntity> sellers);

    TableBuilder<TransactionEntity> TransactionsTable(IEnumerable<TransactionEntity> transactions, bool showType = false, bool showProducts = false);
}
