using System;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Http;

namespace FubuMVC.OwinHost
{
    class OwinCurrentHttpRequest : ICurrentHttpRequest
    {
        private readonly Gate.Request _req;
        private readonly Lazy<string> _baseUrl;

        public OwinCurrentHttpRequest(Gate.Request req)
        {
            _req = req;

            _baseUrl = new Lazy<string>(() => _req.Scheme + "://" + _req.HostWithPort + "/" + _req.PathBase.TrimEnd('/'));
        }

        public string RawUrl()
        {
            return ToFullUrl(_req.Path);
        }

        public string RelativeUrl()
        {
            return _req.Path;
        }

        public string FullUrl()
        {
            var parts = _req.HostWithPort.Split(':');
            var builder = new UriBuilder(_req.Scheme, parts[0]);
            if (parts.Length > 1 && parts[1].Trim().IsNotEmpty())
            {
                var port = 0;
                if (Int32.TryParse(parts[1], out port))
                {
                    builder.Port = port;
                }
            }
            builder.Path = _req.PathBase + _req.Path;
            builder.Query = _req.QueryString;
            return builder.ToString();
        }

        public string ToFullUrl(string url)
        {
            return url.ToAbsoluteUrl(_baseUrl.Value);
        }

        public string HttpMethod()
        {
            return _req.Method;
        }
    }
}