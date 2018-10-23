using SIS.Framework.Api.Contracts;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.Framework.Services.Contracts;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIS.Framework
{
    public static class WebHost
    {
        private const int HostingPort = 8000;

        public static void Start(IMvcApplication application) {
            IDependencyContainer container = new DependencyContainer();
            application.ConfigureServices(container);
            IControllerRouter controllerRouter = new ControllerRouter(container);
            IResourceRouter resourceRouter = container.CreateInstance<IResourceRouter>();
            IRouter router = new Router(controllerRouter, resourceRouter);

            MvcContext.Get.AssemblyName = Assembly
                .GetEntryAssembly()
                .GetName()
                .Name;
            Server server = new Server(HostingPort, router);
            server.Run();
        }
    }
}
