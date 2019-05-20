namespace Prism.Behaviors
{
    internal class DefaultPageBehaviorFactoryOptions : IPageBehaviorFactoryOptions
    {
        public bool UseBottomTabs => true;

        public bool UseSafeArea => true;

        public bool UseChildTitle => true;

        public bool PreferLargeTitles => true;
    }
}