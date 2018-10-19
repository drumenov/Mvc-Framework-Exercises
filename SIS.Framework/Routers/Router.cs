using SIS.Framework.Services.Contracts;
using SIS.Framework.Utilities;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Routers
{
    public class Router : IRouter
    {
        private readonly IControllerRouter htmlHandler;

        private readonly IResourceRouter resourceHandler;


        public Router(IControllerRouter htmlHandler, IResourceRouter resourceHandler) {
            this.htmlHandler = htmlHandler;
            
            this.resourceHandler = resourceHandler;
        }

        public IDependencyContainer DependencyContainer { get; set; }

        public IHttpResponse Handle(IHttpRequest request) {
            IHttpResponse httpResponse;
            if (ResourceUtilities.IsResourceRequest(request.Path)) {
                httpResponse = this.resourceHandler.Handle(request);
            } else {
                httpResponse = this.htmlHandler.Handle(request);
            }
            return httpResponse;
        }
    }
}
