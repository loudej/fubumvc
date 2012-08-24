using System;
using System.Web;
using FubuMVC.Core.Http;
using Gate;

namespace FubuMVC.OwinHost
{
    class OwinCookies : ICookies
    {
        private readonly Lazy<HttpCookieCollection> _requestCookies;
        private readonly Lazy<HttpCookieCollection> _responseCookies;

        public OwinCookies(Request req, Response res)
        {
            _requestCookies = new Lazy<HttpCookieCollection>(
                () =>
                {
                    var cookies = new HttpCookieCollection();
                    foreach (var cookie in req.Cookies)
                    {
                        cookies.Add(new HttpCookie(cookie.Key, cookie.Value));
                    }
                    return cookies;
                });

            _responseCookies = new Lazy<HttpCookieCollection>(
                () => new HttpCookieCollection());
        }

        public HttpCookieCollection Request { get { return _requestCookies.Value; } }
        public HttpCookieCollection Response { get { return _responseCookies.Value; } }

        /// <summary>
        /// Notification method called from response.appendcookie
        /// </summary>
        public void ResponseCookieAdded(HttpCookie cookie)
        {
            Response.Add(cookie);
        }
    }
}