using FubuTestingSupport;
using NUnit.Framework;
using FubuCore;

namespace FubuMVC.OwinHost.Testing
{
    [TestFixture]
    public class FormRequestReaderTester
    {
        [Test]
        public void can_parse_query_string_with_encoding()
        {
            new Gate.Request
                {
                    QueryString = "Anesth=Moore%2C+Roy"
                }.Query["Anesth"].ShouldEqual("Moore, Roy");
        }

        [Test]
        public void can_parse_field_values_in_query_string()
        {
            new Gate.Request
            {
                QueryString = "Moore%2C+Roy=Anesth"
            }.Query["Moore, Roy"].ShouldEqual("Anesth");
        }

        [Test]
        public void can_parse_multiple_values()
        {
            var dict = new Gate.Request
            {
                QueryString = "a=1&b=2&c=3"
            }.Query;

            dict["a"].ShouldEqual("1");
            dict["b"].ShouldEqual("2");
            dict["c"].ShouldEqual("3");

            dict.Count.ShouldEqual(3);
        }
    }
}