using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Artikel")]
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Rahmennummer")]
        public string FrameNumber { get; set; }

        [Display(Name = "Farbe")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Bitte eine Marke auswählen.")]
        public int BrandId { get; set; }

        [Display(Name = "Marke")]
        public Brand Brand { get; set; }

        [Display(Name = "Beschreibung")]
        [Required(ErrorMessage = "Bitte eine Beschreibung eingeben.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Bitte einen Typ auswählen.")]
        public int TypeId { get; set; }

        [Display(Name = "Typ")]
        public ProductType Type { get; set; }

        [Display(Name = "Reifengröße")]
        public string TireSize { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Preis")]
        [Required(ErrorMessage = "Bitte einen Preis eingeben.")]
        public decimal Price { get; set; }

        [Display(Name = "Lagerstatus")]
        public StorageStatus StorageStatus { get; set; }

        [Display(Name = "Wertstatus")]
        public ValueStatus ValueStatus { get; set; }

        [Display(Name = "Etikett")]
        public int? Label { get; set; }

        [Display(Name = "Anmerkungen")]
        public string Notes { get; set; }

        public bool IsEmtpy()
        {
            return string.IsNullOrEmpty(Color)
                && string.IsNullOrEmpty(Description)
                && string.IsNullOrEmpty(TireSize)
                && Brand == null
                && Type == null;
        }

        public bool CanCancel()
        {
            return StorageStatus == StorageStatus.Sold && ValueStatus != ValueStatus.Settled;
        }
        public bool CanSell()
        {
            return StorageStatus == StorageStatus.Available && ValueStatus != ValueStatus.Settled;
        }
        public bool CanSettle()
        {
            return (StorageStatus == StorageStatus.Available || StorageStatus == StorageStatus.Sold) && ValueStatus == ValueStatus.NotSettled;
        }
    }
}
