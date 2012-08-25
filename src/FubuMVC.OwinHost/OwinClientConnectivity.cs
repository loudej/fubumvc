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
            dynamic clientConnected;
            if (_res.Environment.TryGetValue("server.ClientConnected", out clientConnected))
            {
                return clientConnected.IsConnected;
            }
            return true;
        }
    }
}