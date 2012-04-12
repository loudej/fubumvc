using System.Diagnostics;
using Bottles;
using Bottles.BottleLoaders.Assemblies;
using FubuCore;
using FubuMVC.Core;
using NUnit.Framework;
using FubuMVC.StructureMap;
using StructureMap;
using FubuTestingSupport;
using System.Linq;
using System.Collections.Generic;

namespace FubuMVC.Tests
{
    [TestFixture]
    public class can_find_and_load_bottles_with_the_FubuModule_attribute
    {
        [Test]
        public void find_assembly_bottles()
        {
            // Trash gets left over from other tests.  Joy.
            new FileSystem().DeleteFile("something.asset.config");
            new FileSystem().DeleteFile("something.script.config");
            new FileSystem().DeleteFile("else.script.config");
            new FileSystem().DeleteFile("else.asset.config");

            FubuApplication.For(new FubuRegistry()).StructureMap(new Container())
                .Bootstrap();

            var assembly = typeof(AssemblyPackage.AssemblyPackageMarker).Assembly;

            BottlesRegistry.PackageAssemblies.ShouldContain(assembly);

            BottlesRegistry.Bottles.Each(x => Debug.WriteLine(x.Name));


            BottlesRegistry.Bottles.OfType<AssemblyBottleInfo>().Any(x => x.Name == AssemblyBottleInfo.CreateFor(assembly).Name)
                .ShouldBeTrue();
        }
    }
}