using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Artikel")]
    public class Product : IModel
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
        [Range(0.01, double.MaxValue, ErrorMessage = "Bitte einen Preis größer 0,01 eingeben.")]
        public decimal Price { get; set; }

        [Display(Name = "Lagerstatus")]
        public StorageState StorageState { get; set; }

        [Display(Name = "Wertstatus")]
        public ValueState ValueState { get; set; }

        [Display(Name = "Etikett")]
        public int? Label { get; set; }

        public bool IsAllowed(TransactionType transactionType)
        {
            if (ValueState == ValueState.Settled)
            {
                return false;
            }

            switch (transactionType)
            {
                case TransactionType.Acceptance: return false;
                case TransactionType.Cancellation: return StorageState == StorageState.Sold && ValueState != ValueState.Settled;
                case TransactionType.Lock: return StorageState == StorageState.Available || StorageState == StorageState.Sold;
                case TransactionType.MarkAsGone: return StorageState == StorageState.Available;
                case TransactionType.Release: return StorageState == StorageState.Locked || StorageState == StorageState.Gone;
                case TransactionType.Sale: return StorageState == StorageState.Available;
                case TransactionType.Settlement: return StorageState == StorageState.Available || StorageState == StorageState.Sold;
            }

            throw new InvalidOperationException($"Invalid transationType: {transactionType}");
        }
        public void SetState(TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.Acceptance:
                    ValueState = ValueState.NotSettled;
                    StorageState = StorageState.Available;
                    return;
                case TransactionType.Cancellation:
                    StorageState = StorageState.Available;
                    return;
                case TransactionType.Lock:
                    StorageState = StorageState.Locked;
                    return;
                case TransactionType.MarkAsGone:
                    StorageState = StorageState.Gone;
                    return;
                case TransactionType.Release:
                    StorageState = StorageState.Available;
                    return;
                case TransactionType.Sale:
                    StorageState = StorageState.Sold;
                    return;
                case TransactionType.Settlement:
                    ValueState = ValueState.Settled;
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
            return StorageState == StorageState.Gone;
        }
        public bool IsLocked()
        {
            return StorageState == StorageState.Locked;
        }
        public string GetInfoText(IStringLocalizer<SharedResource> localizer)
        {
            Contract.Requires(localizer != null);

            if(IsEmtpy())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.Append(Brand.Name).Append(" - ").AppendLine(Type.Name);
            sb.Append(Description).AppendLine($" {FrameNumber} {Color}");
            return sb.ToString();
        }
    }
}
