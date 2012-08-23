using System;
using System.IO;
using System.Net;
using System.Web;
using FubuCore.Util;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.Compression;
using Gate;

namespace FubuMVC.OwinHost
{
    class OwinHttpWriter : IHttpWriter
    {
        private readonly Response _res;
        private readonly Action<HttpCookie> _cookieAdded;
        private readonly Cache<string, string[]> _headers;

        public OwinHttpWriter(Response res, Action<HttpCookie> cookieAdded)
        {
            _res = res;
            _cookieAdded = cookieAdded;
            _headers = new Cache<string, string[]>(_res.Headers);
        }

        public void AppendHeader(string key, string value)
        {
            _res.Headers.AddHeader(key, value);
        }

        public void WriteFile(string file)
        {
            using (var fileStream = new FileStream(file, FileMode.Open))
            {
                Write(stream => fileStream.CopyTo(stream, 64000));
            }
        }

        public void WriteContentType(string contentType)
        {
            _res.ContentType = contentType;
        }

        public void Write(string content)
        {
            _res.Write(content);
        }

        public void Redirect(string url)
        {
            _res.StatusCode = (int)HttpStatusCode.Redirect;
            _headers["Location"] = new[] { url };
            Write(string.Format("<html><head><title>302 Found</title></head><body><h1>Found</h1><p>The document has moved <a href='{0}'>here</a>.</p></body></html>", url));
        }

        public void WriteResponseCode(HttpStatusCode status, string description = null)
        {
            _res.StatusCode = (int)status;
            _res.ReasonPhrase = description;
        }

        public void AppendCookie(HttpCookie cookie)
        {
            _res.SetCookie(
                cookie.Name,
                new Response.Cookie
                {
                    Value = cookie.Value,
                    Domain = cookie.Domain,
                    Expires = cookie.Expires,
                    HttpOnly = cookie.HttpOnly,
                    Path = cookie.Path,
                    Secure = cookie.Secure
                });
            _cookieAdded(cookie);
        }

        public void UseEncoding(IHttpContentEncoding encoding)
        {
            _res.OutputStream = encoding.Encode(_res.OutputStream);
        }

        public void Write(Action<Stream> output)
        {
            output(_res.OutputStream);
        }

        public void Flush()
        {
            _res.OutputStream.Flush();
        }
    }
}