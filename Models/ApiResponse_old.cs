using System.Net;

namespace Protocolo_web_adm.Models
{
    public class ApiResponse
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}

