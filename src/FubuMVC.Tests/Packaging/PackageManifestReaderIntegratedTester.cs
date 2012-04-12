using System;
using System.IO;
using System.Reflection;
using Bottles;
using Bottles.Diagnostics;
using Bottles.BottleLoaders;
using Bottles.BottleLoaders.Assemblies;
using FubuTestingSupport;
using NUnit.Framework;
using FubuCore;
using System.Linq;
using Rhino.Mocks;

namespace FubuMVC.Tests.Packaging
{
    [TestFixture]
    public class BottleManifestReaderIntegratedTester
    {
        private string packageFolder;
        private BottleManifestReader reader;
        private string theApplicationDirectory = "../../".ToFullPath();
        private LinkedFolderBottleLoader linkedFolderReader;

        [SetUp]
        public void SetUp()
        {
            packageFolder = FileSystem.Combine("../../../TestPackage1").ToFullPath();

            var fileSystem = new FileSystem();
            var manifest = new BottleManifest(){
                Name = "pak1"
            };

            manifest.AddAssembly("TestPackage1");

            fileSystem.PersistToFile(manifest, packageFolder, BottleManifest.FILE);

            linkedFolderReader = new LinkedFolderBottleLoader(theApplicationDirectory, f => f);

            reader = new BottleManifestReader(fileSystem, folder => folder);
        }

        [TearDown]
        public void TearDown()
        {
            new FileSystem().DeleteFile(FileSystem.Combine(theApplicationDirectory, BottleManifest.FILE));
        }

        [Test]
        public void load_a_package_info_from_a_manifest_file_when_given_the_folder()
        {
            // the reader is rooted at the folder location of the main app
            var package = reader.LoadFromFolder("../../../TestPackage1".ToFullPath());

            var assemblyLoader = new AssemblyLoader(new BottlingDiagnostics());
            assemblyLoader.AssemblyFileLoader = file => Assembly.Load(Path.GetFileNameWithoutExtension(file));
            assemblyLoader.LoadAssembliesFromBottle(package);

            var loadedAssemblies = assemblyLoader.Assemblies.ToArray();
            loadedAssemblies.ShouldHaveCount(1);
            loadedAssemblies[0].GetName().Name.ShouldEqual("TestPackage1");
        }

        [Test]
        public void load_a_package_registers_web_content_folder()
        {
            var packageDirectory = "../../../TestPackage1".ToFullPath();
            var package = reader.LoadFromFolder(packageDirectory);
            var directoryContinuation = MockRepository.GenerateMock<Action<string>>();

            package.ForFolder(CommonBottleFiles.WebContentFolder, directoryContinuation);
        
            directoryContinuation.AssertWasCalled(x => x.Invoke(packageDirectory));
        }

		[Test]
		public void load_packages_by_assembly()
		{
			var includes = new BottleManifest();
            
            new FileSystem().PersistToFile(includes, theApplicationDirectory, BottleManifest.FILE);

		    var links = new LinkManifest();
            links.AddLink("../TestPackage1");

            new FileSystem().PersistToFile(links, theApplicationDirectory, LinkManifest.FILE);

			var assemblyLoader = new AssemblyLoader(new BottlingDiagnostics());
            assemblyLoader.AssemblyFileLoader = file => Assembly.Load(Path.GetFileNameWithoutExtension(file));
			
			var package = linkedFolderReader.Load(new BottleLog()).Single();
			assemblyLoader.LoadAssembliesFromBottle(package);

			assemblyLoader
				.Assemblies
				.Single()
				.GetName()
				.Name
				.ShouldEqual("TestPackage1");
		}
    }
}