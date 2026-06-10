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
                // La API devuelve { usuario: { id, ... } }: redirigimos al detalle del socio creado.
                var nuevoId = 0;
                try { using var doc = JsonDocument.Parse(_http.ObtenerBody(response)); if (doc.RootElement.TryGetProperty("usuario", out var us) && us.TryGetProperty("id", out var idEl)) nuevoId = idEl.GetInt32(); } catch { }
                return nuevoId > 0
                    ? RedirectToAction("Detalle", new { id = nuevoId })
                    : RedirectToAction("Create");
            }

            var error = _http.ObtenerMensajeError(response);
            ModelState.AddModelError("", error);
            return View(model);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            var socio = _http.EnviarYDeserializar<JsonElement?>($"api/usuarios/{id}", "GET", token: Token);
            if (socio == null) { TempData["Error"] = "No se encontró el socio."; return RedirectToAction("ListaTodos"); }
            ViewBag.Socio = socio;
            return View();
        }
    }
}
