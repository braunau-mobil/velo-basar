using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Crud;

public sealed class SellerCrudModelFactory
    : ICrudModelFactory<SellerEntity, SellerListParameter>
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IHtmlHelper _htmlHelper;

    public SellerCrudModelFactory(IStringLocalizer<SharedResources> localizer, IHtmlHelper htmlHelper)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
    }

    public async Task<ICrudModel> CreateModelAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        ICrudModel model = new CrudModel<SellerEntity>(entity, CreateEditorAsync, _localizer[VeloTexts.CreateSeller]);
        return await Task.FromResult(model);
    }

    public async Task<ICrudModel> EditModelAsync(SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        ICrudModel model = new CrudModel<SellerEntity>(entity, CreateEditorAsync, _localizer[VeloTexts.EditSeller]);
        return await Task.FromResult(model);
    }

    public Task<ICrudListModel> ListModelAsync(IPaginatedList<CrudItemModel<SellerEntity>> items, SellerListParameter parameter)
       => throw new NotSupportedException();

    private async Task<IHtmlContent> CreateEditorAsync(ViewContext viewContext, SellerEntity entity)
    {
        ArgumentNullException.ThrowIfNull(viewContext);
        ArgumentNullException.ThrowIfNull(entity);

        if (_htmlHelper is IViewContextAware contextAware)
        {
            contextAware.Contextualize(viewContext);
        }

        HtmlContentBuilder result = new();
        result.AppendHtml(await _htmlHelper.PartialAsync("/Views/Seller/_NameEdit.cshtml", entity));
        result.AppendHtml(await _htmlHelper.PartialAsync("/Views/Seller/_AllButNameEdit.cshtml", entity));
        result.AppendHtml(await _htmlHelper.PartialAsync("/Views/Seller/_CommentEdit.cshtml", entity));
        return result;
    }
}
