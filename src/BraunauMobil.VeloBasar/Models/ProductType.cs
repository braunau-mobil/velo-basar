using BraunauMobil.VeloBasar.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Typ")]
    public class ProductType : IModel
    {
        public int Id { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Bitte einen Namen eingeben.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Status")]
        public ObjectState State { get; set; }
    }
}
