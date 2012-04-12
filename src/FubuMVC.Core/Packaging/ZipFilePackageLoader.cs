using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using Bottles.Exploding;
using FubuCore;

namespace FubuMVC.Core.Packaging
{
    public class ZipFilePackageLoader : IBottleLoader
    {
        public IEnumerable<IBottleInfo> Load(IBottleLog log)
        {
            var exploder = BottleExploder.GetBottleExploder(log);
            var reader = new BottleManifestReader(new FileSystem(), GetContentFolderForPackage);

            return FubuMvcPackageFacility.GetPackageDirectories().SelectMany(dir =>
            {
                return exploder.ExplodeDirectory(new ExplodeDirectory(){
                    DestinationDirectory = FubuMvcPackageFacility.GetExplodedPackagesDirectory(),
                    PackageDirectory = dir,
                    Log = log
                });
            }).Select(dir => reader.LoadFromFolder(dir));
        }

        public static string GetContentFolderForPackage(string packageFolder)
        {
            return FileSystem.Combine(packageFolder, CommonBottleFiles.WebContentFolder);
        }
    }
}