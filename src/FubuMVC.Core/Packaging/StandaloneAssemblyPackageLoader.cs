using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using Bottles.BottleLoaders.Assemblies;
using FubuCore;

namespace FubuMVC.Core.Packaging
{
	public class StandaloneAssemblyPackageLoader : IBottleLoader
	{
		private readonly IStandaloneAssemblyFinder _assemblyFinder;

		public StandaloneAssemblyPackageLoader(IStandaloneAssemblyFinder assemblyFinder)
		{
			_assemblyFinder = assemblyFinder;
		}

		public IEnumerable<IBottleInfo> Load(IBottleLog log)
		{
			var assemblies = _assemblyFinder.FindAssemblies(FubuMvcPackageFacility.GetApplicationPath());
            return assemblies.Select(assembly => AssemblyBottleInfo.CreateFor(assembly).As<IBottleInfo>());
		}
	}

    public class StandaloneAssemblyFinder : IStandaloneAssemblyFinder
    {
        public IEnumerable<string> FindAssemblies(string applicationDirectory)
        {
            var assemblyNames = new FileSet { Include = "*.dll", DeepSearch = false };
            var fileSystem = new FileSystem();

            return FubuMvcPackageFacility
                .GetPackageDirectories()
                .SelectMany(dir => fileSystem.FindFiles(dir, assemblyNames));
        }
    }
}