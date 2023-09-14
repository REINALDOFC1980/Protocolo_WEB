using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protocolo_web_adm.Service.IRepository;
using System.Security.Claims;
using RestSharp; // Adicione esta diretiva para importar o namespace do RestSharp
using System.Diagnostics;
using Protocolo_web_adm.Models;
using Newtonsoft.Json;

namespace Protocolo_web_adm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMenuService _Mservice;
        private readonly IDapperServices _Dservice;


        public HomeController(IMenuService services, IDapperServices dservice)
        {
            _Mservice = services;
            _Dservice = dservice;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TriagemProcesso()
        {
            string apiUrl = "https://localhost:7141/api/processo/";
            string token = User.FindFirstValue(ClaimTypes.PrimarySid);

            string usuario = "Nadiane";
            string situacao = "aguardando triagem";

            try
            {
                var client = new RestClient(apiUrl);
                var request = new RestRequest($"BuscarProcessoAll/{usuario}/{situacao}", Method.Get);

                // Adicionar o token no cabeçalho de autorização
                request.AddHeader("Authorization", $"Bearer {token}");

                // Realizar a solicitação GET
                RestResponse response = await client.ExecuteAsync(request);

                // Verificar se a resposta é bem-sucedida (200 OK)
                if (response.IsSuccessful)
                {
                    // Ler e processar a resposta (conteúdo JSON)
                    string responseBody = response.Content;
                    List<ProcessoModel> processos = JsonConvert.DeserializeObject<List<ProcessoModel>>(responseBody);

                    //Montagem do Menu
                    var email = User.FindFirstValue(ClaimTypes.Email);
                    var menusPai = _Mservice.ListaMenuPai(email);
                    foreach (var menuPai in menusPai)
                    {
                        menuPai.SubMenus = _Mservice.ListaMenuFilho(menuPai.MP_id, email);
                    }

                    ViewBag.Menus = menusPai;

                    // Passar os dados dos processos para a View
                    return View(processos);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("Não existe dados para essa pesquisa.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized("Token inválido ou expirado.");
                }
                else
                {
                    return BadRequest($"Erro na requisição: {response.StatusCode} - {response.StatusDescription}");
                }
            }
            catch (Exception ex)
            {
                // Use o tratamento de exceções adequado aqui para retornar respostas apropriadas
                return StatusCode(500, "Ocorreu um erro interno na API.");
            }
        }



        public PartialViewResult Partial_Lista_Protocolo()
        {
            return PartialView();
        }


        public PartialViewResult Partial_ListaDetalhamento()
        {
            return PartialView();
        }


        public IActionResult Teste()
        {
            return View();
        }


    }
}
