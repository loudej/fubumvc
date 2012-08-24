using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Owin;

namespace OwinApplication
{
    /// <summary>
    /// End-user's "YourNamespace.Startup.Configuration" method discovered by convention 
    /// That name may also be set explicitly in config with appSettings key owin:Configuration 
    /// 
    /// It is called by Microsoft.AspNet.Owin pkg for IIS when web.config owin:HandleAllRequests is true.
    /// 
    /// It is also called by Katana.exe when running from commandline for inproc http servers
    /// 
    /// The idea is to have a "web app" project that can run on IIS or process-hosted w/out any changes or recompiling
    /// 
    /// See also, Owin.Loader and Owin.Builder nuget packages (from http://github.com/owin-contrib/owin-hosting)
    /// if you would like to incorporate the same Startup class conventions in other host processes
    /// 
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            // initialize application
            var fubuApp = FubuApplication
                .For<OwinSampleFubuRegistry>()
                .StructureMapObjectFactory();

            // add fubu handler to pipeline
            builder.UseFubu(fubuApp);

            // add another handler to pipeline to respond to all other routes
            builder.UseFunc(_ => ShowEnvironment);
        }

        Task ShowEnvironment(IDictionary<string, object> env)
        {
            var headers = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
            var output = (Stream)env["owin.ResponseBody"];

            headers["Content-Type"] = new[] { "text/plain" };
            using (var writer = new StreamWriter(output))
            {
                writer.WriteLine("Hi there");
                foreach (var kv in env)
                {
                    writer.WriteLine(kv.Key + " " + kv.Value);
                }
            }
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }
    }
}