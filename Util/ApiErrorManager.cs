using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RestSharp;

namespace Protocolo_web_adm.Util
{
    public static class ApiErrorManager
    {
        public static IActionResult HandleErrorResponse(RestResponse response, ModelStateDictionary modelState)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new RedirectResult("/Erro/Erro_500");
            }
            else if (!string.IsNullOrEmpty(response.Content))
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(response.Content, new { status = 0, mensage = "" });

                if (!string.IsNullOrEmpty(errorResponse.mensage))
                {
                    modelState.AddModelError("ApiError", errorResponse.mensage);
                }
            }
            else
            {
                modelState.AddModelError("ApiError", response.Content);
            }

            // Retornar null ou alguma outra ação/mensagem adequada
            return null;
        }
    }
}
