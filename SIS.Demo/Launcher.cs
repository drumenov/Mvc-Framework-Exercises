using SIS.Framework;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.Framework.Services.Contracts;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;
using System;
using System.Reflection;

namespace SIS.Demo
{
    class Launcher
    {
        static void Main(string[] args) {
            IDependencyContainer dependencyContainer = ConfigureDependencyContainer();
            IControllerRouter controllerRouter = new ControllerRouter(dependencyContainer);
            IResourceRouter resourceRouter = dependencyContainer.CreateInstance<IResourceRouter>();
            IRouter router = new Router(controllerRouter, resourceRouter);

            MvcContext.Get.AssemblyName = Assembly
                .GetExecutingAssembly()
                .GetName()
                .Name;

            Server server = new Server(8000, router);
            server.Run();
        }

        private static IDependencyContainer ConfigureDependencyContainer() {
            IDependencyContainer dependencyContainer = new DependencyContainer();
            dependencyContainer.RegisterDependency<IRouter, Router>();
            dependencyContainer.RegisterDependency<IControllerRouter, ControllerRouter>();
            dependencyContainer.RegisterDependency<IResourceRouter, ResourceRouter>();
            dependencyContainer.RegisterDependency<IDependencyContainer, DependencyContainer>();

            return dependencyContainer;
        }
    }
}
