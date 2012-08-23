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
                () =>
                {
                    var cookies = new HttpCookieCollection();
                    // TODO: this collection doesn't fire "add/modify" events - 
                    // how should it inform the response before the output starts writing?
                    return cookies;
                });
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