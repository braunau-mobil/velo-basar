using BraunauMobil.VeloBasar.Resources;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace BraunauMobil.VeloBasar;

public sealed class VeloTexts
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public VeloTexts(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    public LocalizedString AcceptanceCount(int count) => Localize("{0} Annahmen", count);
    public LocalizedString AcceptanceCreateSeller => Localize("Annahme - Verkäufer eingeben");
    public LocalizedString AcceptanceForSellerId(int sellerId) => Localize("Annahme für Verkäufer ID: {0} - Artikel eingeben", sellerId);
    public LocalizedString AcceptanceNumber => Localize("Annahme Nummer");
    public LocalizedString AcceptanceSuccess => Localize("Annahme erfolgreich!");
    public LocalizedString AcceptedProductsAmount(decimal amount) => Localize("Artikel im Wert von {0:C} angenommen", amount);
    public LocalizedString AcceptedProductsCount => Localize("Artikel angenommen");
    public LocalizedString Accepting => Localize("In Annahme");
    public LocalizedString AcceptProducts => Localize("Artikel annehmen");
    public LocalizedString AcceptSessionIsCompleted(int sessionId) => Localize("Die Annahme-Stzung mit der ID {0} ist bereits abgeschlossen", sessionId);
    public LocalizedString AcceptSessionList(int count) => Localize("Annahme Sitzungen ({0})", count);
    public LocalizedString AcceptSessionNotFound(int sessionId) => Localize("Es konnte keine Annahme-Stzung mit der ID {0} gefunden werden", sessionId);
    public LocalizedString AcceptSessions => Localize("Annahme Sitzungen");
    public LocalizedString AceptedProductCount => Localize("Angenommene Artikel");
    public LocalizedString Add => Localize("Hinzufügen");
    public LocalizedString Address => Localize("Anschrift");
    public LocalizedString Admin => Localize("Administration");
    public LocalizedString AdminUserEMail => Localize("Admin User E-Mail (Password = root)");
    public LocalizedString All => Localize("Alle");
    public LocalizedString AllBrand => Localize("Alle (Marke)");
    public LocalizedString AllProductsOfSaleAlreadyCancelledOrSettled => Localize("Es wurden bereits alle Artikel des Verkaufs abgerechnet oder Storniert. Ein Storno ist nicht mehr möglich.");
    public LocalizedString AllProductType => Localize("Alle (Typ)");
    public LocalizedString AmountGiven => Localize("Geld gegeben");
    public LocalizedString AmountGivenTooSmall(decimal amountGiven, decimal saleAmount) => Localize("Der gegebene Betrag {0:C} muss größer als die Verkaufssumme {1:C} sein!", amountGiven, saleAmount);
    public LocalizedString Apply => Localize("Übernehmen");
    public LocalizedString Available => Localize("Verfügbar");
    public LocalizedString BankAccountHolder => Localize("Kontoinhaber");
    public LocalizedString Bankdata => Localize("Bankdaten");
    public LocalizedString Basar => Localize("Basar");
    public LocalizedString BasarDetails => Localize("Basar Details");
    public LocalizedString BasarList => Localize("Basare");
    public LocalizedString Brand => Localize("Marke");
    public LocalizedString BrandList => Localize("Marken");
    public LocalizedString CalculateChangeMoney => Localize("Wechselgeld berechnen");
    public LocalizedString Cancel => Localize("Abbrechen");
    public LocalizedString Cancellate => Localize("Stornieren");
    public LocalizedString Cancellation => Localize("Storno");
    public LocalizedString CannotDeleteBasar(int id) => Localize("Basar mit ID={0} kann nicht gelöscht werden.", id);
    public LocalizedString Cart => Localize("Warenkorb");
    public LocalizedString ChangeMoney => Localize("Wechselgeld");
    public LocalizedString Checkout => Localize("Verkaufen");
    public LocalizedString City => Localize("Stadt");
    public LocalizedString CityIsTooLong(int maxLength) => Localize("Die Stadt darf nicht länger als {0} Zeichen sein.", maxLength);
    public LocalizedString ClearCart => Localize("Warenkorb leeren");
    public LocalizedString Color => Localize("Farbe");
    public LocalizedString Comment => Localize("Kommentar");
    public LocalizedString Continue => Localize("Fortsetzen");
    public LocalizedString ContinueAcceptSession => Localize("Annahme fortsetzen");
    public LocalizedString Cost => Localize("Kosten");
    public LocalizedString Country => Localize("Land");
    public LocalizedString CountryList => Localize("Länder");
    public LocalizedString CreateBasar => Localize("Basar erstellen");
    public LocalizedString CreateBrand => Localize("Marke hinzufügen");
    public LocalizedString CreateCountry => Localize("Land hinzufügen");
    public LocalizedString CreatedAt => Localize("Erstellt");
    public LocalizedString CreateProductType => Localize("Artikel Typ erstellen");
    public LocalizedString CreateSeller => Localize("Verkäufer erstellen");
    public LocalizedString Date => Localize("Datum");
    public LocalizedString Delete => Localize("Löschen");
    public LocalizedString Description => Localize("Beschreibung");
    public LocalizedString Details => Localize("Details");
    public LocalizedString Disable => Localize("Deaktivieren");
    public LocalizedString Document => Localize("Beleg");
    public LocalizedString DonateIfNotSold => Localize("Spenden, falls nicht verkauft");
    public LocalizedString Edit => Localize("Bearbeiten");
    public LocalizedString EditBasar => Localize("Basar bearbeiten");
    public LocalizedString EditBrand => Localize("Marke bearbeiten");
    public LocalizedString EditCountry => Localize("Land bearbeiten");
    public LocalizedString EditProduct => Localize("Artikel bearbeiten");
    public LocalizedString EditProductType => Localize("Artikel Typ bearbeiten");
    public LocalizedString EditSeller => Localize("Verkäufer bearbeiten");
    public LocalizedString EMail => Localize("E-Mail");
    public LocalizedString Empty => Localize("");
    public LocalizedString Enable => Localize("Aktivieren");
    public LocalizedString End => Localize("Ende");
    public LocalizedString EnterProductId => Localize("Artikel ID:");
    public LocalizedString FirstName => Localize("Vorname");
    public LocalizedString FirstNameIsTooLong(int maxLength) => Localize("Der Vorname darf nicht länger als {0} Zeichen sein.", maxLength);
    public LocalizedString FrameNumber => Localize("Rahmennummer");
    public LocalizedString FrameNumberLabel(string frameNumber)
    {
        ArgumentNullException.ThrowIfNull(frameNumber);

        return Localize("Rahmennummer: {0}", frameNumber);
    }
    public LocalizedString GenerateBrands => Localize("Beispiel Marken generieren");
    public LocalizedString GenerateCountries => Localize("Beispiel Länder generieren");
    public LocalizedString GenerateProductTypes => Localize("Beispiel Produkt Typen generieren");
    public LocalizedString GenerateZipCodes => Localize("Postleitzahlen laden");
    public LocalizedString IBAN => Localize("IBAN");
    public LocalizedString Id => Localize("Id");
    public LocalizedString IncomeFromSoldProducts => Localize("Einnahmen aus verkauften Artikeln:");
    public LocalizedString Info => Localize("Info");
    public LocalizedString InitialSetup => Localize("Einrichtung");
    public LocalizedString InvalidLoginAttempt => Localize("Ungüliger Login-Versuch");
    public LocalizedString IsActive => Localize("IsActive");
    public LocalizedString Iso3166Alpha3Code => Localize("ISO-3166 Code");
    public LocalizedString Label => Localize("Etikett");
    public LocalizedString Labels => Localize("Etiketten");
    public LocalizedString LastName => Localize("Nachname");
    public LocalizedString LastNameIsTooLong(int maxLength) => Localize("Der Nachname darf nicht länger als {0} Zeichen sein.", maxLength);
    public LocalizedString Location => Localize("Ort");
    public LocalizedString LocationAndDate(string? location, DateTime timestamp)
    {
        if (location == null)
        {
            return Localize("{1:D} um {1:t}", timestamp);
        }
        return Localize("{0} am {1:D} um {1:t}", location, timestamp);
    }
    public LocalizedString Lock => Localize("Sperren");
    public LocalizedString Locked => Localize("Gesperrt");
    public LocalizedString LockedProductsCount(int count) => Localize("Gesperrte Artikel: {0}", count);
    public LocalizedString LockInfo => Localize("Gesperrte Artikel befinden sich noch auf Lager und können in keiner Transaktion (außer Abrechnung) verwendet werden. Wird ein gesperrter Artikel abgerechnet, wird dieser behandelt wie wenn er nicht verkauft wurde und noch auf Lager ist.");
    public LocalizedString LockProduct(int productId) => Localize("Artikel {0} sperren", productId);
    public LocalizedString LogIn => Localize("Einloggen");
    public LocalizedString LogOut => Localize("Ausloggen");
    public LocalizedString Lost => Localize("Verschwunden");
    public LocalizedString LostInfo => Localize("Verschwundene Artikel befinden sich nicht mehr auf Lager und können in keiner Transaktion (außer Abrechnung) verwendet werden. Wird ein gesperrter Artikel abgerechnet, wird dieser behandelt wie wenn er verkauft wurde.");
    public LocalizedString LostProduct(int productId) => Localize("Artikel {0} als verschwunden markieren", productId);
    public LocalizedString LostProducts(int count) => Localize("Verschwundene Artikel: {0}", count);
    public LocalizedString Menu => Localize("Menü");
    public LocalizedString Misc => Localize("Sonstiges");
    public LocalizedString Name => Localize("Name");
    public LocalizedString NewAcceptance => Localize("Neue Annahme");
    public LocalizedString NewSale => Localize("Neuer Verkauf");
    public LocalizedString Newsletter => Localize("Newsletter");
    public LocalizedString NewsletterPermission => Localize("Newsletter-Erlaubnis");
    public LocalizedString NewsletterPermissionTimesStamp(DateTime? timeStamp)
    {
        if (timeStamp.HasValue)
        {
            return Localize("Newsletter-Erlaubnis erteilt am {0}", timeStamp.Value);
        }
        return Empty;
    }
    public LocalizedString Next => Localize("Weiter");
    public LocalizedString NoAcceptanceFound(int acceptanceNumber) => Localize("Es konnte kein Annahme mit der Nummer {0} gefunden werden.", acceptanceNumber);
    public LocalizedString NoBasarActive => Localize("Kein Basar aktiv");
    public LocalizedString NoProductWithIdFound(int id) => Localize("Es konnte kein Artikel mit der ID {0} gefunden werden.", id);
    public LocalizedString NoSaleFoundWithNumber(int number) => Localize("Es konnte kein Verkauf mit der Nummer {0} gefunden werden", number);
    public LocalizedString NoSellerFound => Localize("Es konnte kein Verkäufer gefunden werden.");
    public LocalizedString Notes => Localize("Anmerkungen");
    public LocalizedString NotSettled => Localize("Nicht abgerechnet");
    public LocalizedString NotSold => Localize("Nicht verkauft");
    public LocalizedString NotSoldProductCount => Localize("Nicht verkaufte Artikel");
    public LocalizedString Number => Localize("Nummer");
    public LocalizedString Ok => Localize("Ok");
    public LocalizedString Other => Localize("Sonstiges");
    public LocalizedString Overview => Localize("Übersicht");
    public LocalizedString PageNumberFromOverall(int number, int pageCount) => Localize("Seite {0} von {1}", number, pageCount);
    public LocalizedString ParentTransaction(TransactionType type, int number) => Localize("Vorgänger: {0} #{1}", Singular(type), number);
    public LocalizedString Password => Localize("Passwort");
    public LocalizedString PhoneNumber => Localize("Telefonnummer");
    public LocalizedString PhoneNumberIsTooLong(int maxLength) => Localize("Die Telefonnummer darf nicht länger als {0} Zeichen sein.", maxLength);
    public LocalizedString PickedUp => Localize("Abgeholt");
    public LocalizedString PickedUpProductCount => Localize("Abgeholte Artikel");
    public LocalizedString PleaseEnterAdminUserEmail => Localize("Bitte eine Admin E-Mail Adresse angeben.");
    public LocalizedString PleaseEnterBrand => Localize("Bitte eine Marke auswählen.");
    public LocalizedString PleaseEnterCity => Localize("Bitte eine Stadt eingeben.");
    public LocalizedString PleaseEnterCountry => Localize("Bitte ein Land auswählen.");
    public LocalizedString PleaseEnterDescription => Localize("Bitte eine Beschreibung eingeben.");
    public LocalizedString PleaseEnterFirstName => Localize("Bitte einen Vornamen eingeben.");
    public LocalizedString PleaseEnterFirstNameForSearch => Localize("Bitte für die Suche einen Vornamen eingeben.");
    public LocalizedString PleaseEnterIso3166Alpha3Code => Localize("Bitte einen ISO Code eingeben.");
    public LocalizedString PleaseEnterLastName => Localize("Bitte einen Nachnamen eingeben.");
    public LocalizedString PleaseEnterLastNameForSearch => Localize("Bitte für die Suche einen Nachnamen eingeben.");
    public LocalizedString PleaseEnterName => Localize("Bitte einen Namen eingeben.");
    public LocalizedString PleaseEnterNotes => Localize("Bitte Anmerkungen eingeben.");
    public LocalizedString PleaseEnterPhoneNumber => Localize("Bitte eine gültige Telefonnummer eingeben.");
    public LocalizedString PleaseEnterProductType => Localize("Bitte einen Typ auswählen.");
    public LocalizedString PleaseEnterStreet => Localize("Bitte eine Straße eingeben.");
    public LocalizedString PleaseEnterValidEMail => Localize("Bitte eine gültige E-Mail Adresse eingeben.");
    public LocalizedString PleaseEnterValidIBAN => Localize("Bitte eine gültige IBAN eingeben.");
    public LocalizedString PleaseEnterValidPercentage => Localize("Bitte einen Wert zwischen 0 und 100 % eingeben");
    public LocalizedString PleaseEnterZIP => Localize("Bitte eine Postleitzahl eingeben.");
    public LocalizedString PleaseSelectProductToCanellate => Localize("Bitte ein Produkt zum Stornieren auswählen.");
    public LocalizedString Price => Localize("Preis");
    public LocalizedString PriceDistribution => Localize("Preisverteilung");
    public LocalizedString PriceMustBeGreaterThanZero => Localize("Bitte einen Preis größer 0,01 eingeben.");
    public LocalizedString PriceMustHavePrecisition => Localize("Es sind nur 2 Nachkommastellen erlaubt.");
    public LocalizedString PrintLabels => Localize("Etiketten drucken");
    public LocalizedString PrintLables => Localize("Etiketten Drucken");
    public LocalizedString PrintTest => Localize("Test-Ausdrucke");
    public LocalizedString Product => Localize("Artikel");
    public LocalizedString ProductCommissionPercentage => Localize("Verkaufs-Provision in Prozent");
    public LocalizedString ProductCount => Localize("Artikelanzahl");
    public LocalizedString ProductCounter(int count) => Localize("{0} Artikel", count);
    public LocalizedString ProductDescription => Localize("Artikelbeschreibung");
    public LocalizedString ProductDescriptionIsTooLong(int maxLength) => Localize("Die Beschreibung darf nicht länger als {0} Zeichen sein.", maxLength);
    public LocalizedString ProductDetails(int id) => Localize("Artikel #{0}", id);
    public LocalizedString ProductIsLocked(string? transactionNotes) => Localize("Der Artikel wurde gesperrt. Anmerkungen: {0}", transactionNotes ?? "");
    public LocalizedString ProductIsLost(string? transactionNotes) => Localize("Der Artikel wurde als verschwunden markiert. Anmerkungen: {0}", transactionNotes ?? "");
    public LocalizedString ProductNotAccepted => Localize("Der Artikel wurde nocht nicht vollständig angenommen.");
    public LocalizedString ProductNotFound(int productId) => Localize("Es konnte kein Artikel mit der ID {0} gefunden werden", productId);
    public LocalizedString Products => Localize("Artikel");
    public LocalizedString ProductSold(string saleUrl, int saleNumber) => Localize("Der Artikel wurde bereits verkauft. Siehe Verkauf <a href=\"{0}\">#{1}</a>", saleUrl, saleNumber);
    public LocalizedString ProductType => Localize("Produkt Typ");
    public LocalizedString ProductTypeList => Localize("Artikel Typen");
    public LocalizedString ProductWasSettled => Localize("Der Artikel wurde bereits abgerechnet.");
    public LocalizedString RememberMe => Localize("Eingeloggt bleiben?");
    public LocalizedString Reset => Localize("Zurücksetzen");
    public LocalizedString ReturnMoney => Localize("Wechselgeld");
    public LocalizedString SaleCount(int count) => Localize("{0} Verkäufe", count);
    public LocalizedString SaleNumber => Localize("Verkaufsnummer");
    public LocalizedString Sales => Localize("Verkäufe");
    public LocalizedString SalesCommision(decimal commissionFactor, decimal total) => Localize("Verkaufsprovision ({0:P2} von {1:C}):", commissionFactor, total);
    public LocalizedString SaleDistribution => Localize("Verteilung Verkaufssummen");
    public LocalizedString SaleSuccesful => Localize("Verkauf erfolgreich.");
    public LocalizedString Save => Localize("Speichern");
    public LocalizedString SaveAcceptSession => Localize("Annahmebeleg speichern");
    public LocalizedString Search => Localize("Suchen");
    public LocalizedString SelectSale => Localize("Verkauf auswählen");
    public LocalizedString Seller => Localize("Verkäufer");
    public LocalizedString SellerCount(int count) => Localize("{0} Verkäufer", count);
    public LocalizedString SellerDetails(int id, string firstName, string lastName) => Localize("Verkäufer #{0} {1} {2}", id, firstName, lastName);
    public LocalizedString SellerId(int id) => Localize("Verkäufer-ID: {0}", id);
    public LocalizedString SellerIdShort(int id) => Localize("Verk.-ID: {0}", id);
    public LocalizedString SellerList => Localize("Verkäufer");
    public LocalizedString SellerNotFound(int sellerId) => Localize("Es konnte kein Verkäufer mit der ID {0} gefunden werden.", sellerId);
    public LocalizedString SellingPrice => Localize("Verkaufspreis");
    public LocalizedString SetAsLost => Localize("Verschwunden");
    public LocalizedString Settle => Localize("Abrechnen");
    public LocalizedString Settled => Localize("$ Abgerechnet");
    public LocalizedString SettlementAmout => Localize("Abgerechneter Betrag");
    public LocalizedString SettlementProgress => Localize("Abrechnungs-Fortschritt");
    public LocalizedString SettlementSuccesful => Localize("Abrechnung erfolgreich.");
    public LocalizedString Size => Localize("Größe");
    public LocalizedString Sold => Localize("$ Verkauft");
    public LocalizedString SoldProductCount => Localize("Verkaufte Artikel");
    public LocalizedString SoldProductsAmount(decimal amount) => Localize("Artikel im Wert von {0:C} verkauft", amount);
    public LocalizedString SoldProductsCount(int count) => Localize("Artikel verkauft: {0}", count);
    public LocalizedString Start => Localize("Start");
    public LocalizedString State => Localize("Status");
    public LocalizedString Street => Localize("Straße");
    public LocalizedString StreetIsTooLong(int maxLength) => Localize("Die Straße darf nicht länger als {0} Zeichen sein.", maxLength);
    public LocalizedString Sum => Localize("Summe");
    public LocalizedString TimeStamp => Localize("Datum und Uhrzeit");
    public LocalizedString TireSize => Localize("Reifengröße");
    public LocalizedString TireSizeLabel(string tireSize)
    {
        ArgumentNullException.ThrowIfNull(tireSize);

        return Localize("Reifengröße: {0}", tireSize);
    }
    public LocalizedString Token => Localize("Token");
    public LocalizedString TotalAmount => Localize("Gesamtbetrag");
    public LocalizedString TransactionList(int count) => Localize("Transaktionen ({0})", count);
    public LocalizedString Transactions => Localize("Transaktionen");
    public LocalizedString TransactionSuccesful(TransactionType type) => Localize("{0} erfolgreich!", Singular(type));
    public LocalizedString TransactionTypeWithNumber(TransactionType type, int number) => Localize("{0} #{1}", Singular(type), number);
    public LocalizedString Type => Localize("Typ");
    public LocalizedString UndefinedProductState => Localize("UNDEFINED PRODUCT STATE");
    public LocalizedString Unlock => Localize("Entsperren");
    public LocalizedString UnlockProduct(int productId) => Localize("Artikel {0} freischalten", productId);
    public LocalizedString Update => Localize("Aktualisieren");
    public LocalizedString UpdatedAt => Localize("Geändert");
    public LocalizedString ValueState => Localize("Abrechnungsstatus");
    public LocalizedString ZIP => Localize("PLZ");

    public LocalizedString Singular(AcceptSessionState? state)
        => state switch
        {
            null => Localize("Alle (Status)"),
            AcceptSessionState.Completed => Localize("Abgeschlossen"),
            AcceptSessionState.Uncompleted => Localize("Laufend"),
            _ => throw new UnreachableException(),
        };

    public LocalizedString Singular(TransactionType? transactionType)
        => transactionType switch
        {
            null => Localize("Alle (Art)"),
            TransactionType.Acceptance => Localize("Annahme"),
            TransactionType.Settlement => Localize("Abrechnung"),
            TransactionType.Unlock => Localize("Freischaltung"),
            TransactionType.Lock => Localize("Sperre"),
            TransactionType.Cancellation => Cancellation,
            TransactionType.Sale => Localize("Verkauf"),
            TransactionType.SetLost => Localize("Verschwunden"),
            _ => throw new UnreachableException(),
        };

    public LocalizedString Plural(TransactionType transactionType)
       => transactionType switch
        {
            TransactionType.Acceptance => Localize("Annahmen"),
            TransactionType.Settlement => Localize("Abrechnungen"),
            TransactionType.Unlock => Localize("Freischaltungen"),
            TransactionType.Lock => Localize("Sperren"),
            TransactionType.Cancellation => Localize("Stornos"),
            TransactionType.Sale => Sales,
            TransactionType.SetLost => Localize("Verschwunden"),
            _ => throw new UnreachableException(),
        };

    public LocalizedString Plural(TransactionType transactionType, int count)
       => transactionType switch
       {
           TransactionType.Acceptance => Localize("Annahmen ({0})", count),
           TransactionType.Settlement => Localize("Abrechnungen ({0})", count),
           TransactionType.Unlock => Localize("Freischaltungen ({0})", count),
           TransactionType.Lock => Localize("Sperren ({0})", count),
           TransactionType.Cancellation => Localize("Stornos ({0})", count),
           TransactionType.Sale => Localize("Verkäufe ({0})", count),
           TransactionType.SetLost => Localize("Verschwunden ({0})", count),
           _ => throw new UnreachableException(),
       };

    public LocalizedString Singular(ValueState? valueState)
      => valueState switch
      {
          null => Localize("Alle (Abrechnungsstatus)"),
          Models.ValueState.NotSettled => Localize("Nicht abgerechnet"),
          Models.ValueState.Settled => Localize("Abgerechnet"),
          _ => throw new UnreachableException(),
      };

    public LocalizedString Singular(ObjectState? objectState)
      => objectState switch
      {
          null => Localize("Alle (Status)"),
          ObjectState.Enabled => Localize("Aktiviert"),
          ObjectState.Disabled=> Localize("Deaktivert"),
          _ => throw new UnreachableException(),
      };

    public LocalizedString Singular(StorageState? storageState)
      => storageState switch
      {
          null => Localize("Alle (Lagerstatus)"),
          StorageState.Available => Localize("Verfügbar"),
          StorageState.Sold => Localize("Verkauft"),
          StorageState.Lost => Localize("Verschwunden"),
          StorageState.Locked => Localize("Gesperrt"),
          StorageState.NotAccepted => Localize("In Annahme"),
          _ => throw new UnreachableException(),
      };

    private LocalizedString Localize(string text, params object[] arguments)
        => _localizer[text, arguments];
}
