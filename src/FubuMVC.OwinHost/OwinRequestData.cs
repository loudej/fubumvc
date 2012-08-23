using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using FubuCore.Binding;
using FubuCore.Binding.Values;
using FubuCore.Util;
using FubuMVC.Core.Http;

namespace FubuMVC.OwinHost
{
    class OwinRequestData : RequestData
    {
        public static readonly string Querystring = "OwinQuerystring";
        public static readonly string FormPost = "OwinFormPost";

        public OwinRequestData(RouteData routeData, Gate.Request req)
        {
            AddValues(new RouteDataValues(routeData));

            AddValues(Querystring, new DictionaryKeyValues(req.Query));
            AddValues(FormPost, new DictionaryKeyValues(req.ReadForm() ?? new Dictionary<string, string>()));

            var headers = req.Headers.ToDictionary(kv => kv.Key, kv => string.Join(",", kv.Value));
            AddValues(RequestDataSource.Header.ToString(), new DictionaryKeyValues(headers));
        }
    }
}