using Microsoft.AspNetCore.Mvc;

namespace gestper.Controllers
{
    public class EncuestaController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Gracias()
        {
            return View();
        }
    }
}