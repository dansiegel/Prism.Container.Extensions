using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Container.Extensions.Shared.Mocks
{
    public class GenericService : IGenericService
    {
        public string Name { get; set; }
    }

    public class AltGenericService : IGenericService
    {
        public string Name { get; set; }
    }

    public interface IGenericService
    {
        string Name { get; }
    }
}
