using FubuMVC.Core;
using FubuMVC.Core.Http.Compression;

namespace OwinApplication
{
    public class OwinSampleFubuRegistry : FubuRegistry
    {
        public OwinSampleFubuRegistry()
        {
            Actions.IncludeClassesSuffixedWithController();

            Views.TryToAttachWithDefaultConventions();

            Import<ContentCompression>(x => x.Exclude(chain => chain.FirstCall().HandlerType != typeof(CompressedContentController)));
        }
    }
}