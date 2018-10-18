using System;
using System.Collections.Generic;
using System.Linq;

namespace SIS.HTTP.Cookies
{
    class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection() {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie httpCookie) {
            if(httpCookie == null) {
                throw new ArgumentNullException();
            }

            //if (this.ContainsCookie(httpCookie.Key)) {
            //    throw new Exception();
            //}

            this.cookies[httpCookie.Key] = httpCookie;
        }

        public bool ContainsCookie(string key) => this.cookies.ContainsKey(key);

        public HttpCookie GetCookie(string key) {
            if (!this.ContainsCookie(key)) {
                return null;
            }
            return this.cookies[key];
        }

        public bool HasCookies() => this.cookies.Count > 0;

        public override string ToString() {
            return String.Join("; ", this.cookies.Values);
        }
    }
}
