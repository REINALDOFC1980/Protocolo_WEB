using Newtonsoft.Json;
using RestSharp;

namespace Protocolo_web_adm.Util
{
    public static class ApiErrorHelper
    {
        public static string GetErrorMessage(RestResponse response)
        {
            return $"Erro na requisição: {response.StatusCode} - {response.StatusDescription}";
        }


    }



}
