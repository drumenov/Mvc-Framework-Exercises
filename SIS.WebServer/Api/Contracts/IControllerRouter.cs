using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Api.Contracts
{
    public interface IControllerRouter
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
