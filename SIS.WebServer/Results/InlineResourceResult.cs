using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content, HttpResponseStatusCode statusCode)
         : base (statusCode){
             this.Headers.Add(new HttpHeader("Content-Length", content.Length.ToString()));
             this.Headers.Add(new HttpHeader("Content-Disposition", "inline"));
             this.Content = content;
        }
    }
}