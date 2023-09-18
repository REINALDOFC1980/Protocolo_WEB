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
        {  //Montagem do Menu
            var email = User.FindFirstValue(ClaimTypes.Email);
            var menusPai = _Mservice.ListaMenuPai(email);

            foreach (var menuPai in menusPai)
            {
                menuPai.SubMenus = _Mservice.ListaMenuFilho(menuPai.MP_id, email);
            }
            ViewBag.Menus = menusPai;
            return View();
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
