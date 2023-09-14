namespace Protocolo_web_adm.Models
{
    public class AutenticacaoModel
    {
        public int id { get; set; }
        public string? autNome { get; set; }
        public string? autEmail { get; set; }
        public string? autSenha { get; set; }
        public string? autRole { get; set; }
        public string? token { get; set; }
    }
    public class LoginResponseModel
    {
        public AutenticacaoModel autenticacaoModel { get; set; }
        public string token { get; set; }
    }
}
