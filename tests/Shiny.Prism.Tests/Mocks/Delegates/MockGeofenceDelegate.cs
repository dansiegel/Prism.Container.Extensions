using System;
using System.Threading.Tasks;
using Shiny.Locations;

namespace Shiny.Prism.Mocks.Delegates
{
    public class MockGeofenceDelegate : IGeofenceDelegate
    {
        public Task OnStatusChanged(GeofenceState newStatus, GeofenceRegion region)
        {
            throw new NotImplementedException();
        }
    }
}
