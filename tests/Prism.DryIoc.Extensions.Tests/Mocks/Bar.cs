namespace Prism.DryIoc.Extensions.Tests
{
    public class Bar : IBar
    {
        public Bar(Foo foo)
        {
            Foo = foo;
        }

        public IFoo Foo { get; }
    }
}
