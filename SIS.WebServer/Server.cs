using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Routing;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    public class Server
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener listener;

        private readonly ServerRoutingTable serverRoutingTable;

        private readonly IHttpHandler router;

        private bool isRunning;

        public Server(int port, IHttpHandler router) {
            this.port = port;
            this.router = router;
            this.listener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), this.port);
        }

        public void Run() {
            this.listener.Start();
            this.isRunning = true;
            Console.WriteLine($"Server started at http://{LocalHostIpAddress}:{this.port}");
            Task task = Task.Run(this.ListenLoop);
            task.Wait();
        }

        public async Task ListenLoop() {
            while (this.isRunning) {
                Socket client = await this.listener.AcceptSocketAsync();
                ConnectionHandler connectionHandler = new ConnectionHandler(client, this.router);
                Task responseTask = connectionHandler.ProcessRequestAsync();
                await responseTask;
            }
        }
    }
}
