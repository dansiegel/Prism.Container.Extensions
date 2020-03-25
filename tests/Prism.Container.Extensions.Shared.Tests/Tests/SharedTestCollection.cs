using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Prism.Container.Extensions.Shared.Tests
{
    public class SharedTests { }

    [Collection(nameof(SharedTests))]
    public class SharedTestCollection : ICollectionFixture<SharedTests>
    {
    }
}
