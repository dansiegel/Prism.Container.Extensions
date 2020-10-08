using System;
using System.Collections.Generic;
using System.Text;
using Prism.Modularity;
using Xunit.Abstractions;

namespace Shiny.Prism.Mocks.Modularity
{
    public class MockModuleStartup : MockStartup
    {
        public MockModuleStartup(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MockShinyPrismModule>();
        }
    }
}
