using System.Diagnostics;

namespace BraunauMobil.VeloBasar.Rendering;

public enum BadgeType
{
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
    Info,
    Light,
    Dark
}

public static class BadgeTypeExtensions
{
    public static string ToCss(this BadgeType type)
        => type switch
    {
        BadgeType.Primary => "text-bg-primary",
        BadgeType.Secondary => "text-bg-secondary",
        BadgeType.Success => "text-bg-success",
        BadgeType.Danger => "text-bg-danger",
        BadgeType.Warning => "text-bg-warning",
        BadgeType.Info => "text-bg-info",
        BadgeType.Light => "text-bg-light",
        BadgeType.Dark => "text-bg-dark",
        _ => throw new UnreachableException()
    };
}