using AutoFixture;
using AutoFixture.Kernel;

namespace BraunauMobil.VeloBasar.Tests;

public class ExcludingEnumBuilder<TEnum>
    : ISpecimenBuilder
    where TEnum : struct, Enum
{
    private static TEnum[] _allEnumValues = Enum.GetValues<TEnum>();

    private readonly EnumGenerator _enumGenerator = new();
    private readonly TEnum[] _valuesToExlude;

    public ExcludingEnumBuilder(params TEnum[] valuesToExlude)
    {
        ArgumentNullException.ThrowIfNull(valuesToExlude);

        valuesToExlude.Should().NotBeEquivalentTo(_allEnumValues);
        _valuesToExlude = valuesToExlude;
    }

    public object Create(object request, ISpecimenContext context)
    {
        object value = _enumGenerator.Create(request, context);
        if (value is TEnum enumValue && _valuesToExlude.Contains(enumValue))
        {
            return Create(request, context);
        }

        return value;
    }
}
