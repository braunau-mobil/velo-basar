using BraunauMobil.VeloBasar.Models;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ProductViewModel
    {
        private readonly ProductsViewModel _parent;

        public ProductViewModel(ProductsViewModel parentViewModel, Product product)
        {
            _parent = parentViewModel;
            Product = product;
        }

        public string Alert { get; set; }
        public bool HasAlert { get; set; }
        public bool ShowSeller { get => _parent.ShowSeller; }
        [Display(Name = "Verkäufer")]
        public int SellerId { get; set; }
        public Product Product { get; }

        public VeloPage GetSellerPage()
        {
            return new VeloPage
            {
                Page = RoutingHelper.GetPageForModel<Pages.Sellers.DetailsModel>(),
                Parameter = new Pages.Sellers.DetailsParameter { SellerId = SellerId }
            };
        }
    }
}
