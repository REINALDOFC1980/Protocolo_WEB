// Interfaces/Services/IApiService.cs
using Newtonsoft.Json.Linq;
using Protocolo_web_adm.Service.IRepository;
using RestSharp;

// Util/ApiService.cs
public class ApiService : IApiService
{
    public async Task<RestResponse> ApiRequestAsyncAutenticacao(string resource, Method method, object data)
    {
            var apiUrl = "https://localhost:7192/api/";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(resource, method);
        
            if (data != null)
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(data);
            }

            return await client.ExecuteAsync(request);
        
    }

    public async Task<RestResponse> ExecuteApiRequestAsync(string resource, Method method, object data, string token)
    {
        {
            var apiUrl = "https://localhost:7286/api/triagem/v1/";           
            var client = new RestClient(apiUrl);
            var request = new RestRequest(resource, method);
            if(token != "")
            request.AddHeader("Authorization", $"Bearer {token}");

            if (data != null)
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(data);
            }

            return await client.ExecuteAsync(request);
        }
    }
}
