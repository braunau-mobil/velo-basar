namespace BraunauMobil.VeloBasar.Tests;

[CollectionDefinition(TestCollections.SampleDatabase)]
public class SampleDatabaseCollection
    : ICollectionFixture<SampleDatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
