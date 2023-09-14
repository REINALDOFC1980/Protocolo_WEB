using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Protocolo_web_adm.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Protocolo_web_adm.Service.IRepository;
using Protocolo_web_adm.Util;
using RestSharp;
using System.Text.Json;

namespace Protocolo_web_adm.Controllers
{

    public class AutenticacaoController : Controller
    {

        private readonly IAutenticacaoServices _service;
        private readonly IApiService _apiService;


        public AutenticacaoController(IAutenticacaoServices services, IApiService apiService)
        {
            _service = services;
            _apiService = apiService;
        }

        public IActionResult Login()
        {

            ClaimsPrincipal claimsPrincipal = HttpContext.User;

            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                return RedirectToAction("TriagemProcesso", "triagem");
            }

            return View();
        }

        public async Task<IActionResult> LogOut(string erro)
        {
            if (!string.IsNullOrEmpty(erro))
            {
                ModelState.AddModelError("", erro);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Autenticacao");
        }


        [HttpPost]
        public async Task<IActionResult> Login(AutenticacaoModel autenticacao)
        {

            try
            {
                var dadosAutenticacao = new
                {
                    autEmail = autenticacao.autEmail,
                    autSenha = autenticacao.autSenha
                };

                var resposta = await _apiService.ApiRequestAsyncAutenticacao("autenticacao", Method.Post, dadosAutenticacao);

                if (resposta.IsSuccessStatusCode)
                {
                    var loginResponse = JsonSerializer.Deserialize<LoginResponseModel>(resposta.Content);

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, loginResponse.autenticacaoModel.autNome),
                        new Claim("Token", loginResponse.token),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        // IsPersistent = autenticacaoModel.ManterLogado
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                    return Ok();// RedirectToAction("TriagemProcesso", "triagem");
                }
                else
                {
                    string errorResponse = resposta.Content;
                    var errorObject = JsonSerializer.Deserialize<ErrorResponseModel>(errorResponse);
                    return StatusCode(400, errorObject.message); // Retorna a mensagem de erro da API

                }

            }
            catch (Exception ex)
            {
                // Handle network, deserialization, or other exceptions
                ModelState.AddModelError("ApiError", $"Erro interno, entre em contato com o administrador: {ex.Message}");
            }

            return View("Login", autenticacao);
        }
    }
}
