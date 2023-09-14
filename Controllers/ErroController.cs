using Microsoft.AspNetCore.Mvc;

namespace Protocolo_web_adm.Controllers
{
    public class ErroController : Controller
    {
        public IActionResult Erro_500()
        {
            // Lógica para lidar com o erro 500
            return View();
        }

        public IActionResult HttpError(int statusCode)
        {
            // Lógica para lidar com outros erros de status
            return View(statusCode);
        }
    }
}
