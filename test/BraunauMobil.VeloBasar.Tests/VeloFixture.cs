namespace BraunauMobil.VeloBasar.Tests;

public class VeloFixture
    : Fixture
{
    public VeloFixture()
    {
#warning @todo generate models that make sense (navigation properties and so)
        Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => Behaviors.Remove(b));
        Behaviors.Add(new OmitOnRecursionBehavior());
    }
}
