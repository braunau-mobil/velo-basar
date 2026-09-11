namespace BraunauMobil.VeloBasar.BusinessLogic;

public enum QrImportStatus
{
    Success,
    Empty,
    InvalidFormat,
    WrongKind,
    UnsupportedSchemaVersion,
    ChecksumMismatch
}

public sealed class QrImportResult
{
    private QrImportResult(QrImportStatus status, string? actualKind, string? unmatchedCountryIso3, string? unmatchedProductType)
    {
        Status = status;
        ActualKind = actualKind;
        UnmatchedCountryIso3 = unmatchedCountryIso3;
        UnmatchedProductType = unmatchedProductType;
    }

    public QrImportStatus Status { get; }

    public string? ActualKind { get; }

    public string? UnmatchedCountryIso3 { get; }

    public string? UnmatchedProductType { get; }

    public static QrImportResult Empty()
        => new(QrImportStatus.Empty, null, null, null);

    public static QrImportResult InvalidFormat()
        => new(QrImportStatus.InvalidFormat, null, null, null);

    public static QrImportResult WrongKind(string? actualKind)
        => new(QrImportStatus.WrongKind, actualKind, null, null);

    public static QrImportResult UnsupportedSchemaVersion()
        => new(QrImportStatus.UnsupportedSchemaVersion, null, null, null);

    public static QrImportResult ChecksumMismatch()
        => new(QrImportStatus.ChecksumMismatch, null, null, null);

    public static QrImportResult Success(string? unmatchedCountryIso3 = null, string? unmatchedProductType = null)
        => new(QrImportStatus.Success, null, unmatchedCountryIso3, unmatchedProductType);
}
