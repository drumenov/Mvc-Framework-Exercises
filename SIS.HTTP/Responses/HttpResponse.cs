using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse() { }

        public HttpResponse(HttpResponseStatusCode statusCode) {
            this.StatusCode = statusCode;
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; }

        public byte[] Content { get; set; }

        public IHttpCookieCollection Cookies { get; set; }

        public void AddCookie(HttpCookie cookie) {
            this.Cookies.Add(cookie);
        }

        public void Addheader(HttpHeader header) {
            this.Headers.Add(header);
        }

        public byte[] GetBytes() {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}");
            builder.AppendLine($"{this.Headers}");
            if (this.Cookies.HasCookies()) {
                builder.AppendLine($"Set-Cookie: {this.Cookies}");
            }
            builder.AppendLine();
            return builder.ToString();
        }
    }
}
