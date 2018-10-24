using SIS.Demo.Services;
using SIS.Demo.Services.Contracts;
using SIS.Framework.Api.Contracts;
using SIS.Framework.Routers;
using SIS.Framework.Services.Contracts;
using SIS.WebServer.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo
{
    public class StartUp : IMvcApplication
    {
        public void Configure() {
            
        }

        public void ConfigureServices(IDependencyContainer dependencyContainer) {
            dependencyContainer.RegisterDependency<IResourceRouter, ResourceRouter>();
            dependencyContainer.RegisterDependency<IUsersService, UsersService>();
            dependencyContainer.RegisterDependency<IAlbumsService, AlbumsService>();
        }
    }
}
