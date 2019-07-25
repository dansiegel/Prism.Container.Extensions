using System;
using Shiny.BluetoothLE;

namespace Shiny.Prism.Mocks.Delegates
{
    public class MockBleAdapterDelegate : IBleAdapterDelegate
    {
        public void OnBleAdapterStateChanged(AccessState state)
        {
            throw new NotImplementedException();
        }
    }
}
