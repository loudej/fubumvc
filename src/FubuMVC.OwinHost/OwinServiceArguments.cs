using System.Threading.Tasks;
using System.Web.Routing;
using FubuCore.Binding;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.AspNet;

namespace FubuMVC.OwinHost
{
    class OwinServiceArguments : ServiceArguments
    {
        public OwinServiceArguments(RouteData routeData, Gate.Request req, Gate.Response res)
        {
            With<IRequestData>(new OwinRequestData(routeData, req));

            var cookies = new OwinCookies(req, res);

            With<ICurrentHttpRequest>(new OwinCurrentHttpRequest(req));
            With<IStreamingData>(new OwinStreamingData(req));
            With<IHttpWriter>(new OwinHttpWriter(res, cookies.ResponseCookieAdded));

            With<IClientConnectivity>(new OwinClientConnectivity(res));

            With<ICookies>(cookies);
        }
    }
}