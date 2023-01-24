using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xan.AspNetCore;

namespace BraunauMobil.VeloBasar;

public static class VeloTexts
{
    private static readonly CultureInfo[] _supportedCultures = new[]
    {
        CultureInfo.GetCultureInfo("en"),
        CultureInfo.GetCultureInfo("de")
    };

    public const string Prefix = "VeloBasar_";

    public const string AcceptanceSingular = $"{Prefix}{nameof(AcceptanceSingular)}";
    public const string AcceptanceCreateSeller = $"{Prefix}{nameof(AcceptanceCreateSeller)}";
    public const string AcceptanceForSellerId = $"{Prefix}{nameof(AcceptanceForSellerId)}";
    public const string AcceptanceNumber = $"{Prefix}{nameof(AcceptanceNumber)}";
    public const string AcceptancesPlural = $"{Prefix}{nameof(AcceptancesPlural)}";
    public const string AcceptancesCount = $"{Prefix}{nameof(AcceptancesCount)}";
    public const string AcceptancesPluralWithCount = $"{Prefix}{nameof(AcceptancesPluralWithCount)}";
    public const string AcceptanceSuccess = $"{Prefix}{nameof(AcceptanceSuccess)}";
    public const string AcceptedProductsAmount = $"{Prefix}{nameof(AcceptedProductsAmount)}";
    public const string AcceptedProducts = $"{Prefix}{nameof(AcceptedProducts)}";
    public const string AcceptedProductsWithCount = $"{Prefix}{nameof(AcceptedProductsWithCount)}";
    public const string Accepting = $"{Prefix}{nameof(Accepting)}";
    public const string AcceptProducts = $"{Prefix}{nameof(AcceptProducts)}";
    public const string AcceptSessionIsCompleted = $"{Prefix}{nameof(AcceptSessionIsCompleted)}";
    public const string AcceptSessionList = $"{Prefix}{nameof(AcceptSessionList)}";
    public const string AcceptSessionNotFound = $"{Prefix}{nameof(AcceptSessionNotFound)}";
    public const string AcceptSessions = $"{Prefix}{nameof(AcceptSessions)}";
    public const string AceptedProductCount = $"{Prefix}{nameof(AceptedProductCount)}";
    public const string Add = $"{Prefix}{nameof(Add)}";
    public const string Address = $"{Prefix}{nameof(Address)}";
    public const string Admin = $"{Prefix}{nameof(Admin)}";
    public const string AdminUserEMail = $"{Prefix}{nameof(AdminUserEMail)}";
    public const string All = $"{Prefix}{nameof(All)}";
    public const string AllBrand = $"{Prefix}{nameof(AllBrand)}";
    public const string AllProductsOfSaleAlreadyCancelledOrSettled = $"{Prefix}{nameof(AllProductsOfSaleAlreadyCancelledOrSettled)}";
    public const string AllProductTypes = $"{Prefix}{nameof(AllProductTypes)}";
    public const string AllStates = $"{Prefix}{nameof(AllStates)}";
    public const string AllStorageStates = $"{Prefix}{nameof(AllStorageStates)}";
    public const string AllTransactionTypes = $"{Prefix}{nameof(AllTransactionTypes)}";
    public const string AllValueStates = $"{Prefix}{nameof(AllValueStates)}";
    public const string AmountGiven = $"{Prefix}{nameof(AmountGiven)}";
    public const string AmountGivenTooSmall = $"{Prefix}{nameof(AmountGivenTooSmall)}";
    public const string Apply = $"{Prefix}{nameof(Apply)}";
    public const string AtDateAndTime = $"{Prefix}{nameof(AtDateAndTime)}";
    public const string AtLocationAndDateAndTime = $"{Prefix}{nameof(AtLocationAndDateAndTime)}";
    public const string Available = $"{Prefix}{nameof(Available)}";
    public const string BankAccountHolder = $"{Prefix}{nameof(BankAccountHolder)}";
    public const string Bankdata = $"{Prefix}{nameof(Bankdata)}";
    public const string Basar = $"{Prefix}{nameof(Basar)}";
    public const string BasarDetails = $"{Prefix}{nameof(BasarDetails)}";
    public const string BasarList = $"{Prefix}{nameof(BasarList)}";
    public const string Brand = $"{Prefix}{nameof(Brand)}";
    public const string BrandList = $"{Prefix}{nameof(BrandList)}";
    public const string CalculateChangeMoney = $"{Prefix}{nameof(CalculateChangeMoney)}";
    public const string Cancel = $"{Prefix}{nameof(Cancel)}";
    public const string Cancellate = $"{Prefix}{nameof(Cancellate)}";
    public const string CancellationSingular = $"{Prefix}{nameof(CancellationSingular)}";
    public const string CancellationsPlural = $"{Prefix}{nameof(CancellationsPlural)}";
    public const string CancellationsPluralWithCount = $"{Prefix}{nameof(CancellationsPluralWithCount)}";
    public const string CannotDeleteBasar = $"{Prefix}{nameof(CannotDeleteBasar)}";
    public const string Cart = $"{Prefix}{nameof(Cart)}";
    public const string ChangeMoney = $"{Prefix}{nameof(ChangeMoney)}";
    public const string Checkout = $"{Prefix}{nameof(Checkout)}";
    public const string City = $"{Prefix}{nameof(City)}";
    public const string CityIsTooLong = $"{Prefix}{nameof(CityIsTooLong)}";
    public const string ClearCart = $"{Prefix}{nameof(ClearCart)}";
    public const string Color = $"{Prefix}{nameof(Color)}";
    public const string Comment = $"{Prefix}{nameof(Comment)}";
    public const string Completed = $"{Prefix}{nameof(Completed)}";
    public const string Continue = $"{Prefix}{nameof(Continue)}";
    public const string ContinueAcceptSession = $"{Prefix}{nameof(ContinueAcceptSession)}";
    public const string Cost = $"{Prefix}{nameof(Cost)}";
    public const string Country = $"{Prefix}{nameof(Country)}";
    public const string CountryList = $"{Prefix}{nameof(CountryList)}";
    public const string CreateBasar = $"{Prefix}{nameof(CreateBasar)}";
    public const string CreateBrand = $"{Prefix}{nameof(CreateBrand)}";
    public const string CreateCountry = $"{Prefix}{nameof(CreateCountry)}";
    public const string CreatedAt = $"{Prefix}{nameof(CreatedAt)}";
    public const string CreateProductType = $"{Prefix}{nameof(CreateProductType)}";
    public const string CreateSeller = $"{Prefix}{nameof(CreateSeller)}";
    public const string Date = $"{Prefix}{nameof(Date)}";
    public const string Delete = $"{Prefix}{nameof(Delete)}";
    public const string Description = $"{Prefix}{nameof(Description)}";
    public const string Details = $"{Prefix}{nameof(Details)}";
    public const string Disable = $"{Prefix}{nameof(Disable)}";
    public const string Document = $"{Prefix}{nameof(Document)}";
    public const string DoLock = $"{Prefix}{nameof(DoLock)}";
    public const string DonateIfNotSold = $"{Prefix}{nameof(DonateIfNotSold)}";
    public const string DoUnlock = $"{Prefix}{nameof(DoUnlock)}";
    public const string Edit = $"{Prefix}{nameof(Edit)}";
    public const string EditBasar = $"{Prefix}{nameof(EditBasar)}";
    public const string EditBrand = $"{Prefix}{nameof(EditBrand)}";
    public const string EditCountry = $"{Prefix}{nameof(EditCountry)}";
    public const string EditProduct = $"{Prefix}{nameof(EditProduct)}";
    public const string EditProductType = $"{Prefix}{nameof(EditProductType)}";
    public const string EditSeller = $"{Prefix}{nameof(EditSeller)}";
    public const string EMail = $"{Prefix}{nameof(EMail)}";
    public const string Empty = $"{Prefix}{nameof(Empty)}";
    public const string Enable = $"{Prefix}{nameof(Enable)}";
    public const string End = $"{Prefix}{nameof(End)}";
    public const string EnterProductId = $"{Prefix}{nameof(EnterProductId)}";
    public const string FirstName = $"{Prefix}{nameof(FirstName)}";
    public const string FirstNameIsTooLong = $"{Prefix}{nameof(FirstNameIsTooLong)}";
    public const string FrameNumber = $"{Prefix}{nameof(FrameNumber)}";
    public const string FrameNumberLabel = $"{Prefix}{nameof(FrameNumberLabel)}";
    public const string GenerateBrands = $"{Prefix}{nameof(GenerateBrands)}";
    public const string GenerateCountries = $"{Prefix}{nameof(GenerateCountries)}";
    public const string GenerateProductTypes = $"{Prefix}{nameof(GenerateProductTypes)}";
    public const string GenerateZipCodes = $"{Prefix}{nameof(GenerateZipCodes)}";
    public const string IBAN = $"{Prefix}{nameof(IBAN)}";
    public const string Id = $"{Prefix}{nameof(Id)}";
    public const string IncomeFromSoldProducts = $"{Prefix}{nameof(IncomeFromSoldProducts)}";
    public const string Info = $"{Prefix}{nameof(Info)}";
    public const string InitialSetup = $"{Prefix}{nameof(InitialSetup)}";
    public const string InvalidLoginAttempt = $"{Prefix}{nameof(InvalidLoginAttempt)}";
    public const string IsActive = $"{Prefix}{nameof(IsActive)}";
    public const string Iso3166Alpha3Code = $"{Prefix}{nameof(Iso3166Alpha3Code)}";
    public const string Label = $"{Prefix}{nameof(Label)}";
    public const string Labels = $"{Prefix}{nameof(Labels)}";
    public const string LastName = $"{Prefix}{nameof(LastName)}";
    public const string LastNameIsTooLong = $"{Prefix}{nameof(LastNameIsTooLong)}";
    public const string Location = $"{Prefix}{nameof(Location)}";
    public const string LockSingular = $"{Prefix}{nameof(LockSingular)}";
    public const string Locked = $"{Prefix}{nameof(Locked)}";
    public const string LockedProductsCount = $"{Prefix}{nameof(LockedProductsCount)}";
    public const string LockInfo = $"{Prefix}{nameof(LockInfo)}";
    public const string LockProduct = $"{Prefix}{nameof(LockProduct)}";
    public const string LocksPlural = $"{Prefix}{nameof(LocksPlural)}";
    public const string LocksPluralWithCount = $"{Prefix}{nameof(LocksPluralWithCount)}";
    public const string LogIn = $"{Prefix}{nameof(LogIn)}";
    public const string LogOut = $"{Prefix}{nameof(LogOut)}";
    public const string Lost = $"{Prefix}{nameof(Lost)}";
    public const string LostInfo = $"{Prefix}{nameof(LostInfo)}";
    public const string LostProduct = $"{Prefix}{nameof(LostProduct)}";
    public const string LostProducts = $"{Prefix}{nameof(LostProducts)}";
    public const string Menu = $"{Prefix}{nameof(Menu)}";
    public const string Misc = $"{Prefix}{nameof(Misc)}";
    public const string Name = $"{Prefix}{nameof(Name)}";
    public const string NewAcceptance = $"{Prefix}{nameof(NewAcceptance)}";
    public const string NewSale = $"{Prefix}{nameof(NewSale)}";
    public const string Newsletter = $"{Prefix}{nameof(Newsletter)}";
    public const string NewsletterPermission = $"{Prefix}{nameof(NewsletterPermission)}";
    public const string NewsletterPermissionTimesStamp = $"{Prefix}{nameof(NewsletterPermissionTimesStamp)}";
    public const string Next = $"{Prefix}{nameof(Next)}";
    public const string NoAcceptanceFound = $"{Prefix}{nameof(NoAcceptanceFound)}";
    public const string NoBasarActive = $"{Prefix}{nameof(NoBasarActive)}";
    public const string NoProductWithIdFound = $"{Prefix}{nameof(NoProductWithIdFound)}";
    public const string NoSaleFoundWithNumber = $"{Prefix}{nameof(NoSaleFoundWithNumber)}";
    public const string NoSellerFound = $"{Prefix}{nameof(NoSellerFound)}";
    public const string NotAccepted = $"{Prefix}{nameof(NotAccepted)}";
    public const string Notes = $"{Prefix}{nameof(Notes)}";
    public const string NotSettled = $"{Prefix}{nameof(NotSettled)}";
    public const string NotSold = $"{Prefix}{nameof(NotSold)}";
    public const string NotSoldProductCount = $"{Prefix}{nameof(NotSoldProductCount)}";
    public const string Number = $"{Prefix}{nameof(Number)}";
    public const string Ok = $"{Prefix}{nameof(Ok)}";
    public const string Other = $"{Prefix}{nameof(Other)}";
    public const string Overview = $"{Prefix}{nameof(Overview)}";
    public const string PageNumberFromOverall = $"{Prefix}{nameof(PageNumberFromOverall)}";
    public const string ParentTransaction = $"{Prefix}{nameof(ParentTransaction)}";
    public const string Password = $"{Prefix}{nameof(Password)}";
    public const string PhoneNumber = $"{Prefix}{nameof(PhoneNumber)}";
    public const string PhoneNumberIsTooLong = $"{Prefix}{nameof(PhoneNumberIsTooLong)}";
    public const string PickedUp = $"{Prefix}{nameof(PickedUp)}";
    public const string PickedUpProductCount = $"{Prefix}{nameof(PickedUpProductCount)}";
    public const string PleaseEnterAdminUserEmail = $"{Prefix}{nameof(PleaseEnterAdminUserEmail)}";
    public const string PleaseEnterBrand = $"{Prefix}{nameof(PleaseEnterBrand)}";
    public const string PleaseEnterCity = $"{Prefix}{nameof(PleaseEnterCity)}";
    public const string PleaseEnterCountry = $"{Prefix}{nameof(PleaseEnterCountry)}";
    public const string PleaseEnterDescription = $"{Prefix}{nameof(PleaseEnterDescription)}";
    public const string PleaseEnterFirstName = $"{Prefix}{nameof(PleaseEnterFirstName)}";
    public const string PleaseEnterFirstNameForSearch = $"{Prefix}{nameof(PleaseEnterFirstNameForSearch)}";
    public const string PleaseEnterIso3166Alpha3Code = $"{Prefix}{nameof(PleaseEnterIso3166Alpha3Code)}";
    public const string PleaseEnterLastName = $"{Prefix}{nameof(PleaseEnterLastName)}";
    public const string PleaseEnterLastNameForSearch = $"{Prefix}{nameof(PleaseEnterLastNameForSearch)}";
    public const string PleaseEnterName = $"{Prefix}{nameof(PleaseEnterName)}";
    public const string PleaseEnterNotes = $"{Prefix}{nameof(PleaseEnterNotes)}";
    public const string PleaseEnterPhoneNumber = $"{Prefix}{nameof(PleaseEnterPhoneNumber)}";
    public const string PleaseEnterProductType = $"{Prefix}{nameof(PleaseEnterProductType)}";
    public const string PleaseEnterStreet = $"{Prefix}{nameof(PleaseEnterStreet)}";
    public const string PleaseEnterValidEMail = $"{Prefix}{nameof(PleaseEnterValidEMail)}";
    public const string PleaseEnterValidIBAN = $"{Prefix}{nameof(PleaseEnterValidIBAN)}";
    public const string PleaseEnterValidPercentage = $"{Prefix}{nameof(PleaseEnterValidPercentage)}";
    public const string PleaseEnterZIP = $"{Prefix}{nameof(PleaseEnterZIP)}";
    public const string PleaseSelectProductToCanellate = $"{Prefix}{nameof(PleaseSelectProductToCanellate)}";
    public const string Price = $"{Prefix}{nameof(Price)}";
    public const string PriceDistribution = $"{Prefix}{nameof(PriceDistribution)}";
    public const string PriceMustBeGreaterThanZero = $"{Prefix}{nameof(PriceMustBeGreaterThanZero)}";
    public const string PriceMustHavePrecisition = $"{Prefix}{nameof(PriceMustHavePrecisition)}";
    public const string PrintLabels = $"{Prefix}{nameof(PrintLabels)}";
    public const string PrintLables = $"{Prefix}{nameof(PrintLables)}";
    public const string PrintTest = $"{Prefix}{nameof(PrintTest)}";
    public const string Product = $"{Prefix}{nameof(Product)}";
    public const string ProductCommissionPercentage = $"{Prefix}{nameof(ProductCommissionPercentage)}";
    public const string ProductCount = $"{Prefix}{nameof(ProductCount)}";
    public const string ProductCounter = $"{Prefix}{nameof(ProductCounter)}";
    public const string ProductDescription = $"{Prefix}{nameof(ProductDescription)}";
    public const string ProductDescriptionIsTooLong = $"{Prefix}{nameof(ProductDescriptionIsTooLong)}";
    public const string ProductDetails = $"{Prefix}{nameof(ProductDetails)}";
    public const string ProductIsLocked = $"{Prefix}{nameof(ProductIsLocked)}";
    public const string ProductIsLost = $"{Prefix}{nameof(ProductIsLost)}";
    public const string ProductNotAccepted = $"{Prefix}{nameof(ProductNotAccepted)}";
    public const string ProductNotFound = $"{Prefix}{nameof(ProductNotFound)}";
    public const string Products = $"{Prefix}{nameof(Products)}";
    public const string ProductSold = $"{Prefix}{nameof(ProductSold)}";
    public const string ProductType = $"{Prefix}{nameof(ProductType)}";
    public const string ProductTypeList = $"{Prefix}{nameof(ProductTypeList)}";
    public const string ProductWasSettled = $"{Prefix}{nameof(ProductWasSettled)}";
    public const string RememberMe = $"{Prefix}{nameof(RememberMe)}";
    public const string Reset = $"{Prefix}{nameof(Reset)}";
    public const string ReturnMoney = $"{Prefix}{nameof(ReturnMoney)}";
    public const string SaleSingular = $"{Prefix}{nameof(SaleSingular)}";
    public const string SaleCount = $"{Prefix}{nameof(SaleCount)}";
    public const string SaleDistribution = $"{Prefix}{nameof(SaleDistribution)}";
    public const string SaleNumber = $"{Prefix}{nameof(SaleNumber)}";
    public const string SalesPlural = $"{Prefix}{nameof(SalesPlural)}";
    public const string SalesCommision = $"{Prefix}{nameof(SalesCommision)}";
    public const string SalesPluralWithCount = $"{Prefix}{nameof(SalesPluralWithCount)}";
    public const string SaleSuccesful = $"{Prefix}{nameof(SaleSuccesful)}";
    public const string Save = $"{Prefix}{nameof(Save)}";
    public const string SaveAcceptSession = $"{Prefix}{nameof(SaveAcceptSession)}";
    public const string Search = $"{Prefix}{nameof(Search)}";
    public const string SelectSale = $"{Prefix}{nameof(SelectSale)}";
    public const string Seller = $"{Prefix}{nameof(Seller)}";
    public const string SellerCount = $"{Prefix}{nameof(SellerCount)}";
    public const string SellerDetails = $"{Prefix}{nameof(SellerDetails)}";
    public const string SellerId = $"{Prefix}{nameof(SellerId)}";
    public const string SellerIdShort = $"{Prefix}{nameof(SellerIdShort)}";
    public const string SellerList = $"{Prefix}{nameof(SellerList)}";
    public const string SellerNotFound = $"{Prefix}{nameof(SellerNotFound)}";
    public const string SellingPrice = $"{Prefix}{nameof(SellingPrice)}";
    public const string SetAsLost = $"{Prefix}{nameof(SetAsLost)}";
    public const string SetLostSingular = $"{Prefix}{nameof(SetLostSingular)}";
    public const string SetLostsPlural = $"{Prefix}{nameof(SetLostsPlural)}";
    public const string SetLostsPluralWithCount = $"{Prefix}{nameof(SetLostsPluralWithCount)}";
    public const string Settle = $"{Prefix}{nameof(Settle)}";
    public const string Settled = $"{Prefix}{nameof(Settled)}";
    public const string SettlementSingular = $"{Prefix}{nameof(SettlementSingular)}";
    public const string SettlementAmout = $"{Prefix}{nameof(SettlementAmout)}";
    public const string SettlementProgress = $"{Prefix}{nameof(SettlementProgress)}";
    public const string SettlementsPlural = $"{Prefix}{nameof(SettlementsPlural)}";
    public const string SettlementsPluralWithCount = $"{Prefix}{nameof(SettlementsPluralWithCount)}";
    public const string SettlementSuccesful = $"{Prefix}{nameof(SettlementSuccesful)}";
    public const string Size = $"{Prefix}{nameof(Size)}";
    public const string Sold = $"{Prefix}{nameof(Sold)}";
    public const string SoldProductCount = $"{Prefix}{nameof(SoldProductCount)}";
    public const string SoldProductsAmount = $"{Prefix}{nameof(SoldProductsAmount)}";
    public const string SoldProductsCount = $"{Prefix}{nameof(SoldProductsCount)}";
    public const string Start = $"{Prefix}{nameof(Start)}";
    public const string State = $"{Prefix}{nameof(State)}";
    public const string Street = $"{Prefix}{nameof(Street)}";
    public const string StreetIsTooLong = $"{Prefix}{nameof(StreetIsTooLong)}";
    public const string Sum = $"{Prefix}{nameof(Sum)}";
    public const string TimeStamp = $"{Prefix}{nameof(TimeStamp)}";
    public const string TireSize = $"{Prefix}{nameof(TireSize)}";
    public const string TireSizeLabel = $"{Prefix}{nameof(TireSizeLabel)}";
    public const string Token = $"{Prefix}{nameof(Token)}";
    public const string TotalAmount = $"{Prefix}{nameof(TotalAmount)}";
    public const string TransactionList = $"{Prefix}{nameof(TransactionList)}";
    public const string Transactions = $"{Prefix}{nameof(Transactions)}";
    public const string TransactionSuccesful = $"{Prefix}{nameof(TransactionSuccesful)}";
    public const string TransactionTypeWithNumber = $"{Prefix}{nameof(TransactionTypeWithNumber)}";
    public const string Type = $"{Prefix}{nameof(Type)}";
    public const string Uncompleted = $"{Prefix}{nameof(Uncompleted)}";
    public const string UndefinedProductState = $"{Prefix}{nameof(UndefinedProductState)}";
    public const string UnlockSingular = $"{Prefix}{nameof(UnlockSingular)}";
    public const string UnlockProduct = $"{Prefix}{nameof(UnlockProduct)}";
    public const string UnlocksPlural = $"{Prefix}{nameof(UnlocksPlural)}";
    public const string UnlocksPluralWithCount = $"{Prefix}{nameof(UnlocksPluralWithCount)}";
    public const string Update = $"{Prefix}{nameof(Update)}";
    public const string UpdatedAt = $"{Prefix}{nameof(UpdatedAt)}";
    public const string ValueState = $"{Prefix}{nameof(ValueState)}";
    public const string ZIP = $"{Prefix}{nameof(ZIP)}";    

    public static string Singular(AcceptSessionState? state)
        => state switch
        {
            null => AllStates,
            AcceptSessionState.Completed => Completed,
            AcceptSessionState.Uncompleted => Uncompleted,
            _ => throw new UnreachableException(),
        };

    public static string Singular(TransactionType? transactionType)
        => transactionType switch
        {
            null => AllTransactionTypes,
            TransactionType.Acceptance => AcceptanceSingular,
            TransactionType.Settlement => SettlementSingular,
            TransactionType.Unlock => UnlockSingular,
            TransactionType.Lock => LockSingular,
            TransactionType.Cancellation => CancellationSingular,
            TransactionType.Sale => SaleSingular,
            TransactionType.SetLost => SetLostSingular,
            _ => throw new UnreachableException(),
        };

    public static string Plural(TransactionType transactionType)
       => transactionType switch
        {
            TransactionType.Acceptance => AcceptancesPlural,
            TransactionType.Settlement => SettlementsPlural,
            TransactionType.Unlock => UnlocksPlural,
            TransactionType.Lock => LocksPlural,
            TransactionType.Cancellation => CancellationsPlural,
            TransactionType.Sale => SalesPlural,
            TransactionType.SetLost => SetLostsPlural,
            _ => throw new UnreachableException(),
        };

    public static string PluralWithCountFormat(TransactionType transactionType)
       => transactionType switch
       {
           TransactionType.Acceptance => AcceptancesPluralWithCount,
           TransactionType.Settlement => SettlementsPluralWithCount,
           TransactionType.Unlock => UnlocksPluralWithCount,
           TransactionType.Lock => LocksPluralWithCount,
           TransactionType.Cancellation => CancellationsPluralWithCount,
           TransactionType.Sale => SalesPluralWithCount,
           TransactionType.SetLost => SetLostsPluralWithCount,
           _ => throw new UnreachableException(),
       };

    public static string Singular(ValueState? valueState)
      => valueState switch
      {
          null => AllValueStates,
          Models.ValueState.NotSettled => NotSettled,
          Models.ValueState.Settled => Settled,
          _ => throw new UnreachableException(),
      };

    public static string Singular(StorageState? storageState)
      => storageState switch
      {
          null => AllStorageStates,
          StorageState.Available => Available,
          StorageState.Sold => Sold,
          StorageState.Lost => Lost,
          StorageState.Locked => Locked,
          StorageState.NotAccepted => NotAccepted,
          _ => throw new UnreachableException(),
      };

    public static void CheckIfAllIsTranslated(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        ResourceManager resourceManager = new("BraunauMobil.VeloBasar.Resources.SharedResources", typeof(SharedResources).Assembly);
        HashSet<string> keys = new(GetTranslationKeys());

        foreach (CultureInfo culture in _supportedCultures)
        {
            ResourceSet? resources = resourceManager.GetResourceSet(culture, true, false);
            if (resources is null)
            {
                logger.LogWarning("No resouces for culture '{CultureName}' found.", culture.Name);
                continue;
            }

            foreach (string key in keys)
            {
                string? value = resources.GetString(key);
                if (value is null)
                {
                    logger.LogWarning("Key '{Key}' is not translated into culture '{CultureName}'.", key, culture.Name);
                }
            }

            foreach (object resource in resources)
            {
                if (resource is DictionaryEntry entry)
                {
                    if (!keys.Contains(entry.Key))
                    {
                        logger.LogWarning("Unused resource '{Key}' for culture '{Culture}'", entry.Key, culture.Name);
                    }
                }
            }
        }
    }

    public static IEnumerable<string> GetTranslationKeys()
    {
        Type veloTextsType = typeof(VeloTexts);
        foreach (FieldInfo field in veloTextsType.GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (field.Name == nameof(Prefix))
            {
                continue;
            }
            if (field.GetValue(null) is not string key)
            {
                continue;
            }

            yield return key;
        }

        Type xanAspNetCoreType = typeof(XanAspNetCoreTexts);
        foreach (FieldInfo field in xanAspNetCoreType.GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (field.Name == nameof(XanAspNetCoreTexts.Prefix))
            {
                continue;
            }
            if (field.GetValue(null) is not string key)
            {
                continue;
            }

            yield return key;
        }
    }
}
