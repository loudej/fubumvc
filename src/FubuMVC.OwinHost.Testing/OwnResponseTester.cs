using System.Net;
using Gate;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuMVC.OwinHost.Testing
{
    [TestFixture]
    public class OwnResponseTester
    {
        private Response response;

        [SetUp]
        protected void beforeEach()
        {
            response = new Response(new Request().Environment);
        }

        [Test]
        public void should_render_response_status_only()
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Status.ShouldEqual("500 Internal Server Error");
        }

        [Test]
        public void should_render_response_status_and_description()
        {
            response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            response.ReasonPhrase = "your mom goes to college";
            response.Status.ShouldEqual("406 your mom goes to college");
        }
    }
}