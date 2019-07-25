namespace Prism.Container.Extensions.Tests.Mocks
{
    public class FooBarImpl : IFoo, IBar
    {
        public string Message { get; set; }
        public IFoo Foo => this;
    }
}
