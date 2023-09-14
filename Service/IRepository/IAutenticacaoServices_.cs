using Protocolo_web_adm.Models;

namespace Protocolo_web_adm.Service.IRepository
{
    public interface IAutenticacaoServices 
    {
        Task<AutenticacaoModel>AutenticarUsuario(AutenticacaoModel autenticacao);       
       
        void AddUsuario(AutenticacaoModel autenticacao);
        void UpdateUsuario(AutenticacaoModel autenticacao);
        void DeleteUsuario(AutenticacaoModel autenticacao);

    }
}
