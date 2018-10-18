using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Sessions;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString) {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString) {
            string[] splitRequestContent = requestString.Split(Environment.NewLine);
            string[] requestLine = splitRequestContent[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (!this.IsValidRequestLine(requestLine)) {
                throw new BadRequestException();
            }
            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();
            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            string[] requestBody = requestString.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            bool hasBody = requestBody.Length > 1;
            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1], hasBody);

        }

        private void ParseCookies() { 
            if (!this.Headers.ContainsHeader("Cookie")) {
                return;
            }
            string cookieHeaderValue = this.Headers.GetHeader("Cookie").Value;
            if (!String.IsNullOrEmpty(cookieHeaderValue)) {
                string[] cookies = cookieHeaderValue.Split("; ", StringSplitOptions.RemoveEmptyEntries);
                foreach(string cookie in cookies) {
                    string[] cookieKeyValuePair = cookie.Split("=", 2);
                    if(cookieKeyValuePair.Length != 2) {
                        throw new BadRequestException();
                    }
                    string cookiKey = cookieKeyValuePair[0];
                    string cookieValue = cookieKeyValuePair[1];
                    this.Cookies.Add(new HttpCookie(cookiKey, cookieValue));
                }
            }
        }

        private void ParseRequestParameters(string bodyParameters, bool hasBody) {
            if (this.Url.Contains("?")) {
                this.ParseQueryParameters(this.Url);
            }

            if (hasBody) {
                this.ParseFormDataParameters(bodyParameters);
            }
        }

        private void ParseQueryParameters(string url) {
            string[] queryParameters = this.Url.Split('?', '#').Skip(1).ToArray();
            if (queryParameters.Length != 0) {
                this.ParseParams(queryParameters[0], this.QueryData);
            }

        }

        private void ParseFormDataParameters(string bodyParameters) {
            this.ParseParams(bodyParameters, this.FormData);
        }

        private void ParseHeaders(string[] headers) {
            if (!headers.Any()) {
                throw new BadRequestException();
            }
            for (int i = 0; i < headers.Length; i++) {
                if (String.IsNullOrEmpty(headers[i])) {
                    return;
                }
                string[] tokens = headers[i].Split(": ", StringSplitOptions.RemoveEmptyEntries);
                HttpHeader header = new HttpHeader(tokens[0], tokens[1]);
                this.Headers.Add(header);
            }
            if (!this.Headers.ContainsHeader("Host")) {
                throw new BadRequestException();
            }
        }

        private void ParseRequestPath() {
            string path = this.Url.Split('?').FirstOrDefault();
            if (String.IsNullOrEmpty(path)) {
                throw new BadRequestException();
            }
            else {
                this.Path = path;
            }
        }

        private void ParseRequestUrl(string[] requestLine) {
            if (String.IsNullOrEmpty(requestLine[1])) {
                throw new BadRequestException();
            }
            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine) {

            if (Enum.TryParse<HttpRequestMethod>(requestLine[0].Capitalize(), out HttpRequestMethod res)) {
                this.RequestMethod = res;
            }
            else {
                throw new BadRequestException();
            }
        }

        private bool IsValidRequestLine(string[] requestLine) {
            if (!requestLine.Any()) {
                throw new BadRequestException();
            }
            return requestLine.Length == 3 && requestLine[2].Equals(GlobalConstants.HttpOneProtocolFragment, StringComparison.OrdinalIgnoreCase);
        }

        private void ParseParams(string @params, Dictionary<string, object> paramsRepo) {
            string[] paramsArray = @params.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (string paramsArr in paramsArray) {
                string[] kvp = paramsArr.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (kvp.Length != 2) {
                    throw new BadRequestException();
                }
                paramsRepo[kvp[0]] = kvp[1];
            }
        }
    }
}
