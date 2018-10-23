using SIS.HTTP.Cookies;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.HTTP.Sessions;
using SIS.WebServer.Api.Contracts;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IRouter router;

        public ConnectionHandler(Socket client, IRouter router) {
            this.client = client;
            this.router = router;
        }

        private string SetRequestSession(IHttpRequest httpRequest) {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey)) {
                HttpCookie cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId) {
            if (!String.IsNullOrEmpty(sessionId)) {
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, $"{sessionId}; HttpOnly"));
            }
        }

        private async Task<IHttpRequest> ReadRequest() {
            StringBuilder builder = new StringBuilder();
            ArraySegment<byte> data = new ArraySegment<byte>(new byte[1024]);

            while (true) {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0) {
                    break;
                }

                string bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                builder.Append(bytesAsString);

                if (numberOfBytesRead < 1023) {
                    break;
                }
            }

            if (builder.Length == 0) {
                return null;
            }

            return new HttpRequest(builder.ToString());
        }        

        
        private async Task PrepareResponse(IHttpResponse httpResponse) {
            byte[] byteSegments = httpResponse.GetBytes();
            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync() {
            IHttpRequest httpRequest = await this.ReadRequest();


            if (httpRequest != null) {
                string sessionId = this.SetRequestSession(httpRequest);
                IHttpResponse httpResponse = this.router.Handle(httpRequest);
                this.SetResponseSession(httpResponse, sessionId);
                await this.PrepareResponse(httpResponse);
            }
            this.client.Shutdown(SocketShutdown.Both);
        }
    }
}