using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocolo_API.Model;
using Protocolo_web_adm.Models;
using Protocolo_web_adm.Service.IRepository;
using Protocolo_web_adm.Util;
using RestSharp;
using System.Diagnostics;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Protocolo_web_adm.Controllers
{
    public class TriagemController : Controller
    {

        private readonly IMenuService _Mservice;
        private readonly IDapperServices _Dservice;
        private readonly IApiService _apiService;


        public TriagemController(IMenuService services, IDapperServices dservice, IApiService apiService)
        {
            _Mservice = services;
            _Dservice = dservice;
            _apiService = apiService;
        }

        public async Task<IActionResult> TriagemProcesso()
        {
      
            //Montagem do Menu
            var email = User.FindFirstValue(ClaimTypes.Email);
            var menusPai = _Mservice.ListaMenuPai(email);

            foreach (var menuPai in menusPai)
            {
                menuPai.SubMenus = _Mservice.ListaMenuFilho(menuPai.MP_id, email);
            }
            ViewBag.Menus = menusPai;

            await ListaTriagemProcesso();

            return View();
        }

        public async Task<PartialViewResult> ListaTriagemProcesso()
        {
            const string usuario = "nadiane";
            const string situacao = "aguardando triagem";

            var token = User.FindFirstValue("Token");//pegando o token gerado!                       
            var processoResponse = await _apiService.ExecuteApiRequestAsync($"buscarprocessoAll/{usuario}/{situacao}", Method.Get, null, token);
            var qtdResponse = await _apiService.ExecuteApiRequestAsync($"buscarquantidadeprocessos/{usuario}", Method.Get, null, token);


            // Validações para a primeira chamada (processos)
            if (processoResponse.IsSuccessful)
            {
                string responseBody = processoResponse.Content;
                List<ProcessoModel> processos = JsonConvert.DeserializeObject<List<ProcessoModel>>(responseBody);
                ViewBag.Processos = processos;
            }
            else
            {
                var errorObj = JsonConvert.DeserializeObject<ErrorResponseModel>(processoResponse.Content);
                ModelState.AddModelError("", "Erro na chamada da API: " + errorObj.message);
            }

            if (qtdResponse.IsSuccessful)
            {
                string qtdresponseBody = qtdResponse.Content;
                List<QuantidadeProcessoModel> qtdprocessos = JsonConvert.DeserializeObject<List<QuantidadeProcessoModel>>(qtdresponseBody);
                ViewBag.QtdProcessos = qtdprocessos;
            }
            

            return PartialView("_ListaProcesso");

        }


        public async Task<PartialViewResult> ProcessoDetalhado(int Pro_id)
        {
            string token = User.FindFirstValue("Token");

            var processoResponse = await _apiService.ExecuteApiRequestAsync($"buscarprocessosId/{Pro_id}", Method.Get, null, token);
            var anexoResponse = await _apiService.ExecuteApiRequestAsync($"buscardocumentosanexado/{Pro_id}", Method.Get, null, token);
            var motcancResponse = await _apiService.ExecuteApiRequestAsync($"buscarmotivoscancelamento", Method.Get, null, token);


            ProcessoModel processos = null;
            List<AnexoModel> anxprocessos = null;
            List<MotivoCancelamentoModel> motivoCancelamentos = null;
            List<MotivoCancelamentoModel> motivoCancelamentosProcesso = null;

            // Tratar a resposta dos processos
            if (processoResponse.IsSuccessful)
            {
                string responseBody = processoResponse.Content;
                processos = JsonConvert.DeserializeObject<ProcessoModel>(responseBody);
            }
            else
            {
                var errorObj = JsonConvert.DeserializeObject<ErrorResponseModel>(processoResponse.Content);
                ModelState.AddModelError("", "Erro na chamada da API: " + errorObj.message);
            }

            // Tratar a resposta dos anexos
            if (anexoResponse.IsSuccessful)
            {
                string anxresponseBody = anexoResponse.Content;
                anxprocessos = JsonConvert.DeserializeObject<List<AnexoModel>>(anxresponseBody);
                //substituir a model por ViewBag.AnxProcessos
            }
            else
            {
                var errorObj = JsonConvert.DeserializeObject<ErrorResponseModel>(processoResponse.Content);
                ModelState.AddModelError("", "Erro na chamada da API: " + errorObj.message);
            }

            // Tratar a resposta dos motivos cancelamento
            if (motcancResponse.IsSuccessful)
            {
                string MotCancBody = motcancResponse.Content;
                motivoCancelamentos = JsonConvert.DeserializeObject<List<MotivoCancelamentoModel>>(MotCancBody);
                //substituir a model por ViewBag.AnxProcessos
            }
            else
            {
                var errorObj = JsonConvert.DeserializeObject<ErrorResponseModel>(processoResponse.Content);
                ModelState.AddModelError("", "Erro na chamada da API: " + errorObj.message);
            }

            var MotivosResponse = await _apiService.ExecuteApiRequestAsync($"processo/buscamotivoscancelamentoprocesso/{Pro_id}", Method.Get, null, token);
            if (MotivosResponse.IsSuccessful)
            {
                string MotCancBody = MotivosResponse.Content;
                motivoCancelamentosProcesso = JsonConvert.DeserializeObject<List<MotivoCancelamentoModel>>(MotCancBody);

            }
            //else
            //{
            //    ApiErrorManager.HandleErrorResponse(MotivosResponse, ModelState);//pegando o erro da API
            //}

            ViewBag.Motivos = motivoCancelamentosProcesso;         
            ViewBag.AnxProcessos = anxprocessos;
            ViewBag.ListaMot_Cancelamento = motivoCancelamentos;
            return PartialView("_ListaDetalhamento", processos);
        }
     
        public async Task<IActionResult> AceitaProcesso(int Pro_id, int resultado)
        {

            var token = User.FindFirstValue("Token");

            //verificando se existe processo
            List<MotivoCancelamentoModel> motivoCancelamentos = null;
            var MotivosResponse = await _apiService.ExecuteApiRequestAsync($"processo/buscamotivoscancelamentoprocesso/{Pro_id}", Method.Get, null, token);
            if (!MotivosResponse.IsSuccessful)
            {
                return StatusCode(400, "É necessário selecionar um motivo.");
            }

            TriagemProcessoModel triagemProcesso = new TriagemProcessoModel
            {
                Tgm_Usuario_Operador = "nadiane",
                Tgm_resul_id = resultado,
                Tgm_Mot_Id = 0,
                Tgm_Pro_Id = Pro_id,
            };


            var triagem_response = await _apiService.ExecuteApiRequestAsync("/processo/InserirResultado", Method.Post, triagemProcesso, token);

            if (!triagem_response.IsSuccessful)
            {
                ApiErrorManager.HandleErrorResponse(triagem_response, ModelState); // Pegando o erro da API               
            }
            await ListaTriagemProcesso();
            return PartialView("_ListaProcesso");

        }

        public async Task<PartialViewResult> InserirMotivoReprovacao(int pro_id, int canc_id)
        {

            var token = User.FindFirstValue("Token");

            MotivoCancelamentoModel motivoCancelamento = new MotivoCancelamentoModel
            {
                Canc_Pro_id = pro_id,
                Canc_Id = canc_id,
                Canc_Usuario = "janeteac",
            };

            var triagem_response = await _apiService.ExecuteApiRequestAsync($"processo/inserirmotivoreprovacao", Method.Post, motivoCancelamento, token);

            if (!triagem_response.IsSuccessful)
            {
                ApiErrorManager.HandleErrorResponse(triagem_response, ModelState); // Pegando o erro da API               
            }


            List<MotivoCancelamentoModel> motivoCancelamentos = null;
            var MotivosResponse = await _apiService.ExecuteApiRequestAsync($"processo/buscamotivoscancelamentoprocesso/{pro_id}", Method.Get, null, token);
            if (MotivosResponse.IsSuccessful)
            {
                string MotCancBody = MotivosResponse.Content;
                motivoCancelamentos = JsonConvert.DeserializeObject<List<MotivoCancelamentoModel>>(MotCancBody);
             }
            else
            {
                ApiErrorManager.HandleErrorResponse(MotivosResponse, ModelState);//pegando o erro da API
            }

            ViewBag.Motivos = motivoCancelamentos;

            return PartialView("_ListaMotivos");

        }


        public async Task<PartialViewResult> DeleteMotivoProcesso(int pro_id, int mov_id)
        {

            //deletanto o motivo
            string token = User.FindFirstValue("Token");
            await _apiService.ExecuteApiRequestAsync($"processo/deletemotivoprocesso/{mov_id}", Method.Delete, null, token);

            //realimentando a lista
            List<MotivoCancelamentoModel> motivoCancelamentosProcesso = null;
            var MotivosResponse = await _apiService.ExecuteApiRequestAsync($"processo/buscamotivoscancelamentoprocesso/{pro_id}", Method.Get, null, token);
            if (MotivosResponse.IsSuccessful)
            { 
                string MotCancBody = MotivosResponse.Content;
                motivoCancelamentosProcesso = JsonConvert.DeserializeObject<List<MotivoCancelamentoModel>>(MotCancBody);
                ViewBag.Motivos = motivoCancelamentosProcesso;
            }

            return PartialView("_ListaMotivos");
        }
    }

}