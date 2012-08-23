using FubuMVC.Core.Http;
using Gate;

namespace FubuMVC.OwinHost
{
    class OwinClientConnectivity : IClientConnectivity
    {
        private readonly Response _res;

        public OwinClientConnectivity(Response res)
        {
            _res = res;
        }

        public bool IsClientConnected()
        {
            dynamic callCompleted;
            if (_res.Environment.TryGetValue("owin.CallCompleted", out callCompleted))
            {
                return callCompleted.IsCompleted;
            }
            return true;
        }
    }
}