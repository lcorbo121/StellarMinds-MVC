using Microsoft.AspNetCore.Mvc;
using StellarMinds.Models.Socios;
using System.Text.Json;
using WebApp.Services.Http;

namespace StellarMinds.Controllers
{
    public class SociosController : Controller
    {
        private readonly AuxiliarClienteHttp _http;

        public SociosController(AuxiliarClienteHttp http)
        {
            _http = http;
        }

        private string? Token => HttpContext.Session.GetString("token");
        private string? Rol => HttpContext.Session.GetString("rol");

        [HttpGet]
        public IActionResult Create()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            return View(new SocioViewModel());
        }

        [HttpGet]
        public IActionResult ListaTodos()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            var usuarios = _http.EnviarYDeserializar<List<JsonElement>>("api/usuarios/todos", "GET", token: Token) ?? [];
            ViewBag.Usuarios = usuarios;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(SocioViewModel model)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            if (!ModelState.IsValid) return View(model);

            var response = _http.EnviarSolicitud("api/usuarios", "POST", new
            {
                model.FullName,
                model.Email,
                model.Calle,
                model.NumeroPuerta,
                model.Apartamento,
                model.Password,
                model.PhoneNumber,
                model.UserName,
                model.RolNombre
            }, Token);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Socio registrado correctamente.";
                return RedirectToAction("Create");
            }

            var error = _http.ObtenerMensajeError(response);
            ModelState.AddModelError("", error);
            return View(model);
        }
    }
}
