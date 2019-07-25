namespace Prism.Container.Extensions.Tests.Mocks
{
    public class Bar : IBar
    {
        public Bar(IFoo foo)
        {
            Foo = foo;
        }

        public IFoo Foo { get; }

        public string Message { get; set; }
    }
}
