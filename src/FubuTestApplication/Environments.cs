using System;
using System.Collections.Generic;
using Bottles.Diagnostics;
using Bottles.Environment;

namespace FubuTestApplication
{
    public class EnvironmentThatBlowsUpInStartUp : IEnvironment
    {
        public void Dispose()
        {
            
        }

        public IEnumerable<IInstaller> StartUp(IBottleLog log)
        {
            throw new ApplicationException("I blew up!");
        }
    }

    public class EnvironmentThatLogsAProblem : IEnvironment
    {
        public void Dispose()
        {

        }

        public IEnumerable<IInstaller> StartUp(IBottleLog log)
        {
            log.MarkFailure("I found a problem in StartUp");
            return new IInstaller[0];
        }
    }

    public class EnvironmentThatBlowsUpInCtor : IEnvironment
    {
        public EnvironmentThatBlowsUpInCtor()
        {
            throw new NotImplementedException("I blew up in the ctor");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IInstaller> StartUp(IBottleLog log)
        {
            throw new NotImplementedException();
        }
    }

    public class EnvironmentWithAllGoodInstallers : IEnvironment
    {
        public void Dispose()
        {
        }

        public IEnumerable<IInstaller> StartUp(IBottleLog log)
        {
            log.Trace("I started up with all good installers");

            yield return new GoodInstaller1();
            yield return new GoodInstaller2();
            yield return new GoodInstaller3();
        }
    }

    public abstract class StubEnvironment : IEnvironment, IInstaller
    {

        public void Dispose()
        {
            
        }

        public IEnumerable<IInstaller> StartUp(IBottleLog log)
        {
            yield return this;
        }

        public virtual void Install(IBottleLog log)
        {
        }

        public virtual void CheckEnvironment(IBottleLog log)
        {
        }
    }


    public class InstallerThatMarksFailureInLogDuringInstall : StubEnvironment
    {
        public override void Install(IBottleLog log)
        {
            log.MarkFailure("I detected a problem during Install");
        }
    }

    public class InstallerThatMarksFailureInLogDuringCheckEnvironment : StubEnvironment
    {
        public override void CheckEnvironment(IBottleLog log)
        {
            log.MarkFailure("I detected a problem during CheckEnvironment");
        }
    }

    public class InstallerThatBlowsUpInCheckEnvironment : StubEnvironment
    {
        public override void CheckEnvironment(IBottleLog log)
        {
            throw new NotImplementedException("The environment is borked!");
        }
    }

    public class InstallerThatBlowsUpInInstall : StubEnvironment
    {
        public override void Install(IBottleLog log)
        {
            throw new NotImplementedException("You shall not pass");
        }
    }

    public class GoodInstaller1 : IInstaller
    {
        public void Install(IBottleLog log)
        {
            log.Trace("All Good 1");
        }

        public void CheckEnvironment(IBottleLog log)
        {
            log.Trace("All Good 1 -- Env");
        }
    }

    public class GoodInstaller2 : IInstaller
    {
        public void Install(IBottleLog log)
        {
            log.Trace("All Good 2");
        }

        public void CheckEnvironment(IBottleLog log)
        {
            log.Trace("All Good 2 -- Env");
        }
    }

    public class GoodInstaller3 : IInstaller
    {
        public void Install(IBottleLog log)
        {
            log.Trace("All Good 3");
        }

        public void CheckEnvironment(IBottleLog log)
        {
            log.Trace("All Good 3 -- Env");
        }
    }
}