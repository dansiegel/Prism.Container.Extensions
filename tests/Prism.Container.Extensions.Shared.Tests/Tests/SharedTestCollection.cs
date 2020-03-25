using Xunit;

namespace Prism.Container.Extensions.Shared.Tests
{
    public class SharedTests { }

    [CollectionDefinition(nameof(SharedTests), DisableParallelization = true)]
    public class SharedTestCollection : ICollectionFixture<SharedTests>
    {
    }
}
