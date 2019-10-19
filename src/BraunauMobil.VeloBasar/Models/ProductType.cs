using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Typ")]
    public class ProductType    
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bitte einen Namen eingeben.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Status")]
        public ModelStatus Status { get; set; }
    }
}
