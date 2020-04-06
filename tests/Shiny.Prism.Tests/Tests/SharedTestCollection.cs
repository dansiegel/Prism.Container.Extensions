using Xunit;

namespace Shiny.Prism.Tests
{
    [CollectionDefinition(nameof(ShinyTests), DisableParallelization = true)]
    public class SharedTestCollection : ICollectionFixture<ShinyTests>
    {
    }
}
