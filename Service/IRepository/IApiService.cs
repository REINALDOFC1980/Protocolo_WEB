using RestSharp;

namespace Protocolo_web_adm.Service.IRepository
{
    public interface IApiService
    {

        Task<RestResponse> ApiRequestAsyncAutenticacao(string resource, Method method, object data);
        Task<RestResponse> ExecuteApiRequestAsync(string resource, Method method, object data, string token);

    }
}
