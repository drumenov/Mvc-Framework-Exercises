using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Api.Contracts
{
    public interface IResourceRouter
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
