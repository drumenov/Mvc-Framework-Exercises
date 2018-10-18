using SIS.Framework.Utilities;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Routers
{
    public class Router : IHttpHandler
    {
        private readonly IHttpHandler htmlHandler;

        private readonly IHttpHandler resourceHandler;

        public Router() {
            this.htmlHandler = new ControllerRouter();
            this.resourceHandler = new ResourceRouter();
        }

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
