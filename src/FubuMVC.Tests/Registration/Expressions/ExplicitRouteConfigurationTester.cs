using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.DSL;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Registration.Expressions
{
    [TestFixture]
    public class when_explicitly_configuring_a_route
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            var config = new ExplicitRouteConfiguration("some/pattern");
            config.Chain();
            _config = config;
            var fubuRegistry = new FubuRegistry(registry =>
            {
                registry.Actions.IncludeTypes(x => false);

                registry.Route("some/pattern")
                    .Calls<InputController>(c => c.DoSomething(null)).OutputToJson();
            });
            _graph = fubuRegistry
                .BuildLightGraph();

            _graph.Behaviors.ShouldHaveCount(1);
            _config.Configure(_graph);
        }

        #endregion

        private IConfigurationAction _config;
        private BehaviorGraph _graph;

        public class InputModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Age2 { get; set; }
        }

        public class OutputModel
        {
        }

        public class InputController
        {
            public OutputModel DoSomething(InputModel input)
            {
                return new OutputModel();
            }
        }

        [Test]
        public void should_add_new_behavior_node_to_graph()
        {
            _graph.Behaviors.ShouldHaveCount(2);

            var visitor = new BehaviorVisitor(new NulloConfigurationObserver(), "");
            visitor.Filters += chain => !chain.Calls.Any(call => call.InputType() == typeof (InputModel));
            visitor.Actions += chain => chain.Top.ShouldBeNull();

            _graph.VisitBehaviors(visitor);
        }
    }
}