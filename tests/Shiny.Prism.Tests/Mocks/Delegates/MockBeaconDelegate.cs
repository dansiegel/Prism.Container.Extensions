using System;
using System.Threading.Tasks;
using Shiny.Beacons;

namespace Shiny.Prism.Mocks.Delegates
{
    public class MockBeaconDelegate : IBeaconDelegate
    {
        public Task OnStatusChanged(BeaconRegionState newStatus, BeaconRegion region)
        {
            throw new NotImplementedException();
        }
    }
}
