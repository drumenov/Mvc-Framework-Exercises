using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection() {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header) {
            if (header != null 
                && !String.IsNullOrEmpty(header.Key) 
                && !String.IsNullOrEmpty(header.Value)
                && !this.ContainsHeader(header.Key)) {
                this.headers.Add(header.Key, header);
            } else {
                throw new Exception();
            }
        }

        public bool ContainsHeader(string key) {
            if (String.IsNullOrEmpty(key)) {
                throw new ArgumentNullException($"{nameof(key)} cannot be null.");
            }
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key) {
            if (this.ContainsHeader(key)) {

                return this.headers[key];
            } else {
                return null;
            }
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            foreach(KeyValuePair<string, HttpHeader> kvp in this.headers) {
                builder.AppendLine(kvp.Value.ToString());
            }
            return builder.ToString().TrimEnd();
        }
    }
}
