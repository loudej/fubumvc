using System;
using Bottles;
using Bottles.Diagnostics;
using FubuLocalization.Basic;
using FubuMVC.Core.Localization;
using FubuTestingSupport;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace FubuMVC.Tests.Localization
{
    [TestFixture]
    public class SpinUpLocalizationCachesTester : InteractionContext<SpinUpLocalizationCaches>
    {
        private List<IBottleInfo> thePackages;

        protected override void beforeEach()
        {
            thePackages = new List<IBottleInfo>();
            ClassUnderTest.Activate(thePackages, MockFor<IBottleLog>());
        }

        [Test]
        public void should_load_the_caches()
        {
            MockFor<ILocalizationProviderFactory>().AssertWasCalled(x => x.LoadAll(null), x => x.Constraints(Is.NotNull()));
        }

        [Test]
        public void should_apply_the_factory_to_localization_manager()
        {
            MockFor<ILocalizationProviderFactory>().AssertWasCalled(x => x.ApplyToLocalizationManager());
        }
    }
}