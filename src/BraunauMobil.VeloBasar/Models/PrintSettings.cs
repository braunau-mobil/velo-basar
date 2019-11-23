using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Models
{
    public class Margins
    {
        public Margins(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        [Display(Name = "Links")]
        public int Left { get; set; }
        [Display(Name = "Oben")]
        public int Top { get; set; }
        [Display(Name = "Rechts")]
        public int Right { get; set; }
        [Display(Name = "Unten")]
        public int Bottom { get; set; }
    }

    public class TransactionPrintSettings
    {
        [Display(Name = "Titel")]
        public string TitleFormat { get; set; }
        [Display(Name = "Unterschrift")]
        public string SignatureFormat { get; set; }
        [Display(Name = "Untertitel")]
        public string SubTitle { get; set; }
        [Display(Name = "Token")]
        public string TokenFormat { get; set; }

        public string GetSignatureText(ProductsTransaction transaction, IStringLocalizer<SharedResource> localizer)
        {
            Contract.Requires(transaction != null);
            Contract.Requires(localizer != null);

            return string.Format(CultureInfo.CurrentCulture, SignatureFormat, "______________________________");
        }
        public string GetTokenText(Seller seller)
        {
            Contract.Requires(seller != null);

            return string.Format(CultureInfo.CurrentCulture, TokenFormat, seller.Token);
        }
    }

    public class PrintSettings
    {
        public PrintSettings()
        {
            Acceptance = new TransactionPrintSettings
            {
                TitleFormat = "Annahmebeleg: Braunau mobil - {0}",
                SignatureFormat = "Für Braunau mobil: {0}",
                SubTitle = "Folgende Ihrer Artikel haben wir in unseren Verkauf aufgenommen:",
                TokenFormat = "Ihr Token für den Online-Login: {0}"
            };
            PageMargins = new Margins(20, 10, 20, 10);

            Transactions.Add(TransactionType.Acceptance, Acceptance);
        }

        public IDictionary<TransactionType, TransactionPrintSettings> Transactions { get; private set; } = new Dictionary<TransactionType, TransactionPrintSettings>();

        [Display(Name = "Annahme")]
        public TransactionPrintSettings Acceptance { get; set; }
        [Display(Name = "Seitenränder")]
        public Margins PageMargins { get; set; }
    }
}
