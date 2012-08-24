using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Routing;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using Gate;

namespace FubuMVC.OwinHost
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class FubuOwinHost
    {
        private readonly AppFunc _app;
        private IList<RouteBase> _routes;

        public bool Verbose { get; set; }

        public FubuOwinHost(AppFunc app, IApplicationSource applicationSource)
        {
            _app = app;
            _routes = applicationSource.BuildApplication().Bootstrap().Routes;
        }
        
        public FubuOwinHost(AppFunc app, FubuApplication fubuApplication)
        {
            _app = app;
            _routes = fubuApplication.Bootstrap().Routes;
        }

        public FubuOwinHost(AppFunc app, FubuRuntime fubuRuntime)
        {
            _app = app;
            _routes = fubuRuntime.Routes;
        }

        public FubuOwinHost(AppFunc app, IList<RouteBase> routes)
        {
            _app = app;
            _routes = routes;
        }


        public Task Invoke(IDictionary<string, object> env)
        {
            var req = new Request(env);
            var routeData = determineRouteData(req);
            if (routeData == null)
            {
                return _app(env);
            }

            if (Verbose)
            {
                Console.WriteLine("Received {0} - {1}", req.Method, req.Path);
            }

            var res = new Response(env);
            var task = executeRoute(routeData, req, res);

            if (Verbose)
            {
                task = task.ContinueWith(t => Console.WriteLine(" ({0})", res.StatusCode));
            }

            return task;
        }

        private static Task executeRoute(RouteData routeData, Gate.Request req, Gate.Response res)
        {
            var arguments = new OwinServiceArguments(routeData, req, res);
            var invoker = routeData.RouteHandler.As<FubuRouteHandler>().Invoker;

            var exceptionHandlingObserver = new ExceptionHandlingObserver();
            arguments.Set(typeof(IExceptionHandlingObserver), exceptionHandlingObserver);

            invoker.Invoke(arguments, routeData.Values);
            var task = Task.Factory.StartNew(() => invoker.Invoke(arguments, routeData.Values));

            return task.ContinueWith(x =>
            {
                try
                {
                    x.FinishProcessingTask(exceptionHandlingObserver);
                }
                catch (Exception ex)
                {
                    write500(res, ex);
                }
            });
        }

        private static void write500(Gate.Response res, Exception ex)
        {
            res.StatusCode = (int)HttpStatusCode.InternalServerError;
            res.Write("FubuMVC has detected an exception\r\n");
            res.Write(ex.ToString());
        }

        private RouteData determineRouteData(Gate.Request req)
        {
            var context = new GateHttpContext(req.Path, req.Method);
            foreach (var route in _routes)
            {
                var routeData = route.GetRouteData(context);
                if (routeData != null)
                {
                    return routeData;
                }
            }
            return null;
        }

        private static void write404(Gate.Response res)
        {
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Write("Sorry, I can't find this resource");
        }
    }
}
