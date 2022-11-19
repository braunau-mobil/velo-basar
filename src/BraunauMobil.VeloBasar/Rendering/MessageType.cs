using System.Diagnostics;

namespace BraunauMobil.VeloBasar.Rendering;

public enum MessageType
{
    Success,
    Danger,
    Warning,
    Info
}

public static class MessageTypeExtensions
{
    public static string ToCss(this MessageType type)
        => type switch
    {
        MessageType.Success => "alert-success",
        MessageType.Danger => "alert-danger",
        MessageType.Warning => "alert-warning",
        MessageType.Info => "alert-primary",
        _ => throw new UnreachableException(),
    };
}