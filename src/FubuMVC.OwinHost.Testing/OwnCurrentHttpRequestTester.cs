using System.Collections.Generic;
using Gate;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuMVC.OwinHost.Testing
{
    [TestFixture]
    public class OwnCurrentHttpRequest_FullUrl_Tester
    {
        private OwinCurrentHttpRequest currentHttpRequest;
        private Request request;

        [SetUp]
        public void SetUp()
        {
            request = new Request
                           {
                               Scheme = "https",
                               Host = "localhost"
                           };
            currentHttpRequest = new OwinCurrentHttpRequest(request);
        }

        [Test]
        public void should_prepend_scheme()
        {
            currentHttpRequest.FullUrl().ShouldStartWith("https://");
        }

        [Test]
        public void should_support_host_without_port()
        {
            currentHttpRequest.FullUrl().ShouldStartWith("https://localhost");
        }

        [Test]
        public void should_support_host_with_port()
        {
            request.Headers.SetHeader("Host", "localhost:8080");
            currentHttpRequest.FullUrl().ShouldStartWith("https://localhost:8080");
        }

        [Test]
        public void should_support_ip_address_with_port()
        {
            request.Headers.SetHeader("Host", "127.0.0.1:8080");
            currentHttpRequest.FullUrl().ShouldStartWith("https://127.0.0.1:8080");
        }

        [Test]
        public void should_be_tolerant_of_invalid_host_format()
        {
            request.Headers.SetHeader("Host", "localhost:  ");
            currentHttpRequest.FullUrl().ShouldStartWith("https://localhost");
        }

        [Test]
        public void should_ignore_invalid_port()
        {
            request.Headers.SetHeader("Host", "localhost:a");
            currentHttpRequest.FullUrl().ShouldStartWith("https://localhost");
        }

        [Test]
        public void should_use_path()
        {
            request.Path = "/foo";
            currentHttpRequest.FullUrl().ShouldEqual("https://localhost/foo");
        }

        [Test]
        public void should_support_querystring()
        {
            request.QueryString = "baz=foo";
            currentHttpRequest.FullUrl().ShouldEqual("https://localhost/?baz=foo");
        }

        [Test]
        public void should_support_path_and_querystring()
        {
            request.Path = "/foo";
            request.QueryString = "baz=foo";
            currentHttpRequest.FullUrl().ShouldEqual("https://localhost/foo?baz=foo");
        }
    }
}