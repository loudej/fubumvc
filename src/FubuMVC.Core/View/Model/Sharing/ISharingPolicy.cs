using Bottles.Diagnostics;

namespace FubuMVC.Core.View.Model.Sharing
{
    public interface ISharingPolicy
    {
        void Apply(IBottleLog log, ISharingRegistration registration);
    }
}