using System;
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

        public bool IsAllowed(TransactionType transactionType)
        {
            if (ValueStatus == ValueStatus.Settled)
            {
                return false;
            }

            switch (transactionType)
            {
                case TransactionType.Acceptance: return false;
                case TransactionType.Cancellation: return StorageStatus == StorageStatus.Sold && ValueStatus != ValueStatus.Settled;
                case TransactionType.Lock: return StorageStatus == StorageStatus.Available || StorageStatus == StorageStatus.Sold;
                case TransactionType.MarkAsGone: return StorageStatus == StorageStatus.Available;
                case TransactionType.Release: return StorageStatus == StorageStatus.Locked || StorageStatus == StorageStatus.Gone;
                case TransactionType.Sale: return StorageStatus == StorageStatus.Available;
                case TransactionType.Settlement: return StorageStatus == StorageStatus.Available || StorageStatus == StorageStatus.Sold;
            }

            throw new InvalidOperationException($"Invalid transationType: {transactionType}");
        }
        public void SetState(TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.Acceptance:
                    ValueStatus = ValueStatus.NotSettled;
                    StorageStatus = StorageStatus.Available;
                    return;
                case TransactionType.Cancellation:
                    StorageStatus = StorageStatus.Available;
                    return;
                case TransactionType.Lock:
                    StorageStatus = StorageStatus.Locked;
                    return;
                case TransactionType.MarkAsGone:
                    StorageStatus = StorageStatus.Gone;
                    return;
                case TransactionType.Release:
                    StorageStatus = StorageStatus.Available;
                    return;
                case TransactionType.Sale:
                    StorageStatus = StorageStatus.Sold;
                    return;
                case TransactionType.Settlement:
                    ValueStatus = ValueStatus.Settled;
                    return;
            }

            throw new InvalidOperationException($"Invalid transationType: {transactionType}");
        }


        public bool IsEmtpy()
        {
            return string.IsNullOrEmpty(Color)
                && string.IsNullOrEmpty(Description)
                && string.IsNullOrEmpty(TireSize)
                && Brand == null
                && Type == null;
        }
        public bool IsGone()
        {
            return StorageStatus == StorageStatus.Gone;
        }
        public bool IsLocked()
        {
            return StorageStatus == StorageStatus.Locked;
        }
    }
}
