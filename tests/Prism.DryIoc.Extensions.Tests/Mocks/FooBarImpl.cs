using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.DryIoc.Extensions.Tests.Mocks
{
    class FooBarImpl : IFoo, IBar
    {
        public string Message { get; set; }
        public IFoo Foo => this;
    }
}
