using Bottles;

namespace FubuMVC.Core.Packaging
{
    public static class PackageLoadingConfigurationExtensions
    {
        public static void Bootstrapper<T>(this IBottleFacility configuration) where T : IBootstrapper, new()
        {
            configuration.Bootstrapper(new T());
        }

        public static void Loader<T>(this IBottleFacility configuration) where T : IBottleLoader, new()
        {
            configuration.Loader(new T());
        }

        public static void Activator<T>(this IBottleFacility configuration) where T : IActivator, new()
        {
            configuration.Activator(new T());
        }

        public static void Facility<T>(this IBottleFacility configuration) where T : BottleFacility, new()
        {
            configuration.Facility(new T());
        }
    }
}