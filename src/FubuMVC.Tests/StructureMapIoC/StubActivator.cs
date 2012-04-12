using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;

namespace FubuMVC.Tests.StructureMapIoC
{
    public class StubActivator : IActivator
    {
        private IEnumerable<IBottleInfo> _packages;
        private IBottleLog _log;

        public void Activate(IEnumerable<IBottleInfo> packages, IBottleLog log)
        {
            _packages = packages;
            _log = log;


        }

        public IEnumerable<IBottleInfo> Packages
        {
            get { return _packages; }
        }

        public IBottleLog Log
        {
            get { return _log; }
        }
    }
}