using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Models
{
    public class Margins
    {
        public Margins() { }
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

    public abstract class TransactionPrintSettingsBase
    {
        [Display(Name = "Titel")]
        public string TitleFormat { get; set; }
        public abstract TransactionType TransactionType { get; }
    }

    public class AcceptancePrintSettings : TransactionPrintSettingsBase
    {
        [Display(Name = "Unterschrift")]
        public string SignatureText { get; set; }
        [Display(Name = "Untertitel")]
        public string SubTitle { get; set; }
        [Display(Name = "Token")]
        public string TokenFormat { get; set; }
        public override TransactionType TransactionType => TransactionType.Acceptance;

        public string GetTokenText(Seller seller)
        {
            if (seller == null) throw new ArgumentNullException(nameof(seller));

            return string.Format(CultureInfo.CurrentCulture, TokenFormat, seller.Token);
        }
    }

    public class SalePrintSettings : TransactionPrintSettingsBase
    {
        [Display(Name = "Dankestext")]
        public string FooterText { get; set; }
        [Display(Name ="Hinweistext")]
        public string HintText { get; set; }
        [Display(Name = "Verkäufer Fußnotentext")]
        public string SellerInfoText { get; set; }
        [Display(Name = "Untertitel")]
        public string SubTitle { get; set; }

        public override TransactionType TransactionType => TransactionType.Sale;
    }

    public class SettlementPrintSettings : TransactionPrintSettingsBase
    {
        [Display(Name = "Bestätigungstext")]
        public string ConfirmationText { get; set; }
        [Display(Name = "Unterschrift")]
        public string SignatureText { get; set; }
        [Display(Name = "Überschrift verkaufte Artikeltabelle")]
        public string SoldTitle { get; set; }
        [Display(Name = "Überschfitt nicht-verkaufte Artikeltabelle")]
        public string NotSoldTitle { get; set; }

        public override TransactionType TransactionType => TransactionType.Settlement;
    }

    public class LabelPrintSettings
    {
        public string TitleFormat { get;  set; }
    }

    public class PrintSettings
    {
        public PrintSettings()
        {
            Acceptance = new AcceptancePrintSettings
            {
                TitleFormat = "Braunau mobil - {0} : Annahmebeleg #{1}",
                SignatureText = "Für Braunau mobil:",
                SubTitle = "Folgende Ihrer Artikel haben wir in unseren Verkauf aufgenommen:",
                TokenFormat = "Ihr Token für den Online-Login: {0}"
            };
            Sale = new SalePrintSettings
            {
                TitleFormat = "Braunau mobil - {0} : Verkaufsbeleg #{1}",
                SubTitle = "Folgende Artikel haben wir an Sie verkauft:",
                FooterText = "Vielen Dank für Ihren Einkauf!",
                HintText = "Bitte beachten Sie, dass der Verkauf im Namen des beim Artikel angeführten Eigentümers geschieht. Bei privaten Verkäufern ist jegliches Umtausch - und Gewährleistungsrecht ausgeschlossen.",
                SellerInfoText = "*Kontaktdaten des Verkäufers"
            };
            Settlement = new SettlementPrintSettings
            {
                ConfirmationText = "Hiermit bestätige ich, dass ich meine unverkauften Artikel zurück erhalten habe und den angegebenen Verkaufserlös erhalten habe.",
                NotSoldTitle = "Folgende Ihrer Artikel konnten wir leider nicht verkaufen:",
                SignatureText = "Unterschrift",
                SoldTitle = "Folgende Ihrer Artikel haben wir für Sie verkauft:",
                TitleFormat = "Braunau mobil - {0} : Abrechung #{1}"
            };
            Label = new LabelPrintSettings
            {
                TitleFormat = "Braunau mobil - {0}"
            };
            PageMargins = new Margins(20, 20, 20, 20);
            BannerSubtitle = "Braunau mobil, Hans-Sachs Straße 33, 5280 Braunau";
            Website = "www.braunaumobil.at";
            Banner = new ImageData();
        }

        [Display(Name = "Annahme")]
        public AcceptancePrintSettings Acceptance { get; set; }
        [Display(Name = "Verkauf")]
        public SalePrintSettings Sale { get; set; }
        [Display(Name = "Abrechnung")]
        public SettlementPrintSettings Settlement { get; set; }
        [Display(Name = "Etiketten")]
        public LabelPrintSettings Label { get; set; }
        [Display(Name = "Seitenränder")]
        public Margins PageMargins { get; set; }
        [Display(Name = "Banner")]
        public ImageData Banner { get; set; }

        [Display(Name = "Bannertext")]
        public string BannerSubtitle { get; set; }
        [Display(Name = "Website")]
        public string Website { get; set; }
    }
}
