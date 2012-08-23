using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Routing;
using FubuCore;
using FubuMVC.Core.Runtime;
using Gate;

namespace FubuMVC.OwinHost
{
    public class FubuOwinHost
    {
        public bool Verbose { get; set; }

        public Task Invoke(IDictionary<string, object> env)
        {
            var req = new Request(env);
            var res = new Response(env);



            if (Verbose) Console.WriteLine("Received {0} - {1}", req.Method, req.Path);

            Task task;
            var routeData = determineRouteData(req);
            if (routeData == null)
            {
                write404(res);
                task = TaskHelpers.Completed();
            }
            else
            {
                task = executeRoute(routeData, req, res);
            }

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
            return RouteTable.Routes.GetRouteData(context);
        }

        private static void write404(Gate.Response res)
        {
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Write("Sorry, I can't find this resource");
        }
    }
}
