using FakeItEasy;
using FakeItEasy.Creation;

namespace BraunauMobil.VeloBasar.Tests;

class MakeEverythingStrictOptionsBuilder
    : IFakeOptionsBuilder
{
    public bool CanBuildOptionsForFakeOfType(Type _)
        => true;

    public void BuildOptions(Type _, IFakeOptions options)
       => options.Strict();

    public Priority Priority => Priority.Default;
}
