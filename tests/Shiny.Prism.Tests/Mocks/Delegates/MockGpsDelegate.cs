using System;
using System.Threading.Tasks;
using Shiny.Locations;

namespace Shiny.Prism.Mocks.Delegates
{
    public class MockGpsDelegate : IGpsDelegate
    {
        public Task OnReading(IGpsReading reading)
        {
            throw new NotImplementedException();
        }
    }
}
