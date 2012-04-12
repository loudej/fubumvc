using System.Collections.Generic;
using Bottles;
using Bottles.Deployment.Runtime.Content;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Core.Packaging;
using FubuMVC.Deployers;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuMVC.Tests.Deployers
{
    [TestFixture]
    public class FubuBottleDestinationTester
    {
        private FubuBottleDestination theDestination;
        private string theRootFolder;

        [SetUp]
        public void SetUp()
        {
            theRootFolder = "deployedPath";
            theDestination = new FubuBottleDestination(theRootFolder);
        }

        [Test]
        public void create_request_for_binaries()
        {
            var request = theDestination.DetermineExplosionRequests(new BottleManifest(){
                Role = BottleRoles.Binaries,
                Name = "the bottle name"
            }).Single();

            request.BottleDirectory.ShouldEqual(CommonBottleFiles.BinaryFolder);
            request.DestinationDirectory.ShouldEqual(FileSystem.Combine(theRootFolder, CommonBottleFiles.BinaryFolder));
            request.BottleName.ShouldEqual("the bottle name");
        }

        [Test]
        public void create_request_for_config()
        {
            var request = theDestination.DetermineExplosionRequests(new BottleManifest()
            {
                Role = BottleRoles.Config,
                Name = "the bottle name"
            }).Single();

            request.BottleDirectory.ShouldEqual(CommonBottleFiles.ConfigFolder);
            request.DestinationDirectory.ShouldEqual(FileSystem.Combine(theRootFolder, CommonBottleFiles.ConfigFolder));
            request.BottleName.ShouldEqual("the bottle name");
        }

        [Test]
        public void create_request_for_module()
        {
            var requests = theDestination.DetermineExplosionRequests(new BottleManifest()
            {
                Role = BottleRoles.Module,
                Name = "the bottle name"
            });

            var firstRequest = requests.First();

            firstRequest.BottleDirectory.ShouldBeNull();
            var thePackagesFolder = theRootFolder.AppendPath(CommonBottleFiles.BinaryFolder,
                                              FubuMvcPackageFacility.FubuPackagesFolder);

            firstRequest.DestinationDirectory.ShouldEqual(thePackagesFolder);

            var secondRequet = requests.Skip(1).First();
            secondRequet.BottleDirectory.ShouldEqual(CommonBottleFiles.BinaryFolder);
            secondRequet.DestinationDirectory.ShouldEqual(theRootFolder.AppendPath(CommonBottleFiles.BinaryFolder));
        }

        [Test]
        public void create_requests_for_module()
        {
            var requests = theDestination.DetermineExplosionRequests(new BottleManifest()
            {
                Role = BottleRoles.Application,
                Name = "the bottle name"
            });

            var expected = new List<BottleExplosionRequest>{
                new BottleExplosionRequest(new BottleLog()){BottleName = "the bottle name", BottleDirectory = CommonBottleFiles.BinaryFolder, DestinationDirectory = FileSystem.Combine(theRootFolder, CommonBottleFiles.BinaryFolder)},
                new BottleExplosionRequest(new BottleLog()){BottleName = "the bottle name", BottleDirectory = CommonBottleFiles.WebContentFolder, DestinationDirectory = theRootFolder},
            };
           
            requests.ShouldHaveTheSameElementsAs(expected);
        }
    }
}