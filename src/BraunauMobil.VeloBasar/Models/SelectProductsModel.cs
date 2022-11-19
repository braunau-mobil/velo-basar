using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace BraunauMobil.VeloBasar.Models;

public sealed class SelectProductsModel
    : AbstractActiveBasarModel
{
    public int TransactionId { get; set; }

    [SuppressMessage("Design", "CA1002:Do not expose generic lists")]
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
    public List<SelectModel<ProductEntity>> Products { get; set; } = new List<SelectModel<ProductEntity>>();

    public void SetProducts(IReadOnlyList<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        Products = new List<SelectModel<ProductEntity>>(products.Select(product => new SelectModel<ProductEntity>(product)));
    }

    public IEnumerable<int> SelectedProductIds()
        => Products.Where(selectModel => selectModel.IsSelected)
        .Select(selectModel => selectModel.Value)
        .Ids();
}

public sealed class SelectProductsModelValidator
    : AbstractValidator<SelectProductsModel>
{
    private readonly VeloTexts _txt;

    public SelectProductsModelValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(model => model.Products)
            .Custom(ValidatProducts);
    }

    private void ValidatProducts(List<SelectModel<ProductEntity>> products, ValidationContext<SelectProductsModel> context)
    {
        if (!products.Any(vm => vm.IsSelected))
        {
            context.AddFailure(_txt.PleaseSelectProductToCanellate);
        }
    }
}
