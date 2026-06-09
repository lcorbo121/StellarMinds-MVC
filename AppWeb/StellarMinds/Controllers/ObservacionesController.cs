using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Services.Http;

namespace StellarMinds.Controllers
{
    public class ObservacionesController : Controller
    {
        private readonly AuxiliarClienteHttp _http;

        public ObservacionesController(AuxiliarClienteHttp http)
        {
            _http = http;
        }

        private string? Token => HttpContext.Session.GetString("token");
        private string? Rol => HttpContext.Session.GetString("rol");
        private int UserId => HttpContext.Session.GetInt32("userId") ?? 0;

        [HttpGet]
        public IActionResult Ranking()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            var ranking = _http.EnviarYDeserializar<List<JsonElement>>("api/observaciones/ranking", "GET", token: Token) ?? [];
            ViewBag.Ranking = ranking;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Evaluar(int prestamoId, int objetoCelesteId)
        {
            if (Token == null) return Json(new { error = "No autorizado" });
            var resp = _http.EnviarSolicitud("api/observaciones/evaluar", "POST", new { prestamoId, objetoCelesteId }, Token, throwOnError: false);
            var json = _http.ObtenerBody(resp);
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, json);
            return Content(json, "application/json");
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Socio" && Rol != "Administrador") return Forbid();
            var objetos = _http.EnviarYDeserializar<List<JsonElement>>("api/observaciones/objetos-celestes", "GET", token: Token) ?? [];
            ViewBag.ObjetosJson = JsonSerializer.Serialize(objetos);
            if (Rol == "Administrador")
            {
                var socios = _http.EnviarYDeserializar<List<JsonElement>>("api/usuarios", "GET", token: Token) ?? [];
                ViewBag.SociosJson = JsonSerializer.Serialize(socios);
                ViewBag.PrestamosJson = "[]";
            }
            else
            {
                var prestamos = _http.EnviarYDeserializar<List<JsonElement>>($"api/prestamos/vigentes-socio/{UserId}", "GET", token: Token) ?? [];
                ViewBag.PrestamosJson = JsonSerializer.Serialize(prestamos);
                ViewBag.SociosJson = "[]";
            }
            return View();
        }

        [HttpGet]
        public IActionResult MisObservaciones()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Socio") return Forbid();
            var lista = _http.EnviarYDeserializar<List<JsonElement>>("api/observaciones/mis-observaciones", "GET", token: Token, throwOnError: false) ?? [];
            ViewBag.Observaciones = lista;
            return View();
        }

        [HttpGet]
        public IActionResult TodasObservaciones()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Coordinador" && Rol != "Administrador") return Forbid();
            var lista = _http.EnviarYDeserializar<List<JsonElement>>("api/observaciones/todas", "GET", token: Token, throwOnError: false) ?? [];
            ViewBag.Observaciones = lista;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(int prestamoId, int objetoCelesteId, string fechaObservacion, string notas, string indicadorIA, string detalleIA)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Socio" && Rol != "Administrador") return Forbid();
            if (prestamoId == 0 || objetoCelesteId == 0 || string.IsNullOrEmpty(fechaObservacion))
            {
                TempData["Error"] = "Debe completar todos los campos obligatorios.";
                return RedirectToAction("Create");
            }
            var dto = new { prestamoId, objetoCelesteId, fechaObservacion, notas = notas ?? "", indicadorIA = indicadorIA ?? "", detalleIA = detalleIA ?? "" };
            var resp = _http.EnviarSolicitud("api/observaciones", "POST", dto, Token, throwOnError: false);
            var body = _http.ObtenerBody(resp);
            if (!resp.IsSuccessStatusCode)
            {
                try { var err = System.Text.Json.JsonDocument.Parse(body); TempData["Error"] = err.RootElement.TryGetProperty("error", out var e) ? e.GetString() : body; }
                catch { TempData["Error"] = body; }
                return RedirectToAction("Create");
            }
            TempData["ResultadoIA"] = body;
            TempData["Exito"] = "Observación registrada correctamente.";
            return RedirectToAction("Create");
        }
    }
}
