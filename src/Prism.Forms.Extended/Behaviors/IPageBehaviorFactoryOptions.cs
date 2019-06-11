namespace Prism.Behaviors
{
    public interface IPageBehaviorFactoryOptions
    {
        bool UseBottomTabs { get; }

        bool UseSafeArea { get; }

        bool UseChildTitle { get; }

        bool PreferLargeTitles { get; }
    }
}