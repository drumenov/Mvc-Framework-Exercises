using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters;

        public HttpSession(string id) {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter) {
            if (this.ContainsParameter(name) || parameter == null) {
                throw new ArgumentException();
            }
            this.parameters[name] = parameter;
        }

        public void ClearParameters() {
            this.parameters.Clear();
        }

        public bool ContainsParameter(string name) => this.parameters.ContainsKey(name);

        public object GetParameter(string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException();
            }
            if (!this.ContainsParameter(name)) {
                return null;
            }

            return this.parameters[name];
        }
    }
}
