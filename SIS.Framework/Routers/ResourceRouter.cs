using SIS.Framework.Utilities;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.Framework.Routers
{
    public class ResourceRouter : IResourceRouter
    {
        public IHttpResponse Handle(IHttpRequest request) {
            string resourceExtension = ResourceUtilities.GetResourceExtension(request.Path);
            string resourceFullName = ResourceUtilities.GetResourceFullName(request.Path, resourceExtension);
            string resourceFile = $"{MvcContext.Get.ResourcesFolder}/{resourceExtension.Substring(1)}/{resourceFullName}";
            if (!File.Exists(resourceFile)) {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }
            byte[] content = File.ReadAllBytes(resourceFile);

            return new InlineResourceResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
