using SIS.Framework;
using SIS.Framework.Routers;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;
using System.Reflection;

namespace SIS.Demo
{
    class Launcher
    {
        static void Main(string[] args) {
            IHttpHandler router = new Router();

            MvcContext.Get.AssemblyName = Assembly
                .GetExecutingAssembly()
                .GetName()
                .Name;

            Server server = new Server(8000, router);
            server.Run();
        }
    }
}
