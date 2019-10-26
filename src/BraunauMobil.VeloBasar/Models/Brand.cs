using BraunauMobil.VeloBasar.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Marke")]
    public class Brand : IModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bitte einen Namen eingeben.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Status")]
        public ObjectState State { get; set; }
    }
}
