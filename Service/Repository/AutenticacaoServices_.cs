using Dapper;
using Microsoft.AspNetCore.Mvc;
using Protocolo_web_adm.Models;
using Protocolo_web_adm.Service.IRepository;
using Protocolo_web_adm.Util;
using RestSharp;
using System.Data;
using System.Text.Json;

namespace Protocolo_web_adm.Service.Repository
{
    public class AutenticacaoServices : IAutenticacaoServices
    {
        private readonly IDapperServices _dapper;

        public AutenticacaoServices(IDapperServices dapper)
        {
            _dapper = dapper;
        }        

        public async Task<AutenticacaoModel> AutenticarUsuario(AutenticacaoModel autenticacao)
        {
            var cliente = new RestClient("https://localhost:7141");
            var Aut_Email = autenticacao.autEmail;
            var Aut_Senha = autenticacao.autSenha;

            var requisicao = new RestRequest("/api/autenticacao/login", Method.Post);
            requisicao.AddHeader("Content-Type", "application/json");

            var dadosAutenticacao = new
            {
                Aut_Email,
                Aut_Senha
            };

            requisicao.AddJsonBody(dadosAutenticacao);

            try
            {
                var resposta = await cliente.ExecuteAsync(requisicao);

                if (resposta.IsSuccessful)
                {
                    string content = resposta.Content ?? string.Empty;

                    if (!string.IsNullOrEmpty(content))
                    {
                        var respostaModel = JsonSerializer.Deserialize<RespostaAutenticacaoModel>(content);
                        autenticacao.token = respostaModel.token;
                        return autenticacao;
                    }
                }
                else
                {
                    string errorMessage = ApiErrorHelper.GetErrorMessage(resposta);
                    Console.WriteLine($"Erro na autenticação: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Lidar com exceções de rede, desserialização, etc.
                Console.WriteLine($"Erro na autenticação: {ex.Message}");
            }

            return null;
        }




        public void DeleteUsuario(AutenticacaoModel autenticacao)
        {
            throw new NotImplementedException();
        }

        public void UpdateUsuario(AutenticacaoModel autenticacao)
        {
            throw new NotImplementedException();
        }

        public void AddUsuario(AutenticacaoModel autenticacao)
        {
            throw new NotImplementedException();
        }
    }
}
