using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IColorProvider
{
    Color Primary { get; }

    Color this[string key] { get; }
}
