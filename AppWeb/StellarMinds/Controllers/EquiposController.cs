using Microsoft.AspNetCore.Mvc;
using StellarMinds.Models.Equipos;
using System.Text.Json;
using WebApp.Services.Http;

namespace StellarMinds.Controllers
{
    public class EquiposController : Controller
    {
        private readonly AuxiliarClienteHttp _http;
        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        public EquiposController(AuxiliarClienteHttp http)
        {
            _http = http;
        }

        private string? Token => HttpContext.Session.GetString("token");
        private string? Rol => HttpContext.Session.GetString("rol");

        private string UrlTipo(string tipo) => tipo.ToLower() switch
        {
            "telescopio" or "telescopios" => "api/telescopios",
            "montura" or "monturas" => "api/monturas",
            "camara" or "camaras" => "api/camaras",
            "ocular" or "oculares" => "api/oculares",
            _ => throw new ArgumentException("Tipo inválido")
        };

        public IActionResult Index(string tipo = "telescopios")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            var json = _http.ObtenerBody(_http.EnviarSolicitud(UrlTipo(tipo), "GET", token: Token));
            ViewBag.Tipo = tipo.ToLower();
            ViewBag.Equipos = json;
            return View();
        }

        [HttpGet]
        public IActionResult Create(string tipo = "telescopios")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            return tipo.ToLower() switch
            {
                "montura" or "monturas" => View("CreateMontura", new MonturaViewModel()),
                "camara" or "camaras" => View("CreateCamara", new CamaraViewModel()),
                "ocular" or "oculares" => View("CreateOcular", new OcularViewModel()),
                _ => View("CreateTelescopio", new TelescopioViewModel())
            };
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateTelescopio(TelescopioViewModel model, string tipo = "telescopios")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("CreateTelescopio", model);
            var dto = new { model.Marca, model.Modelo, model.Apertura, model.RelacionFocal, model.DistanciaFocal, model.Peso, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud(UrlTipo(tipo), "POST", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Telescopio creado correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("CreateTelescopio", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateMontura(MonturaViewModel model, string tipo = "monturas")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("CreateMontura", model);
            var dto = new { model.Marca, model.Modelo, model.TipoMontura, model.CargaUtil, model.EsGoTo, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud(UrlTipo(tipo), "POST", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Montura creada correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("CreateMontura", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateCamara(CamaraViewModel model, string tipo = "camaras")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("CreateCamara", model);
            var dto = new { model.Marca, model.Modelo, model.TipoSensor, model.Resolucion, model.TamanoPixel, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud(UrlTipo(tipo), "POST", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Cámara creada correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("CreateCamara", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateOcular(OcularViewModel model, string tipo = "oculares")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("CreateOcular", model);
            var dto = new { model.Marca, model.Modelo, model.Diametro, model.AnguloVision, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud(UrlTipo(tipo), "POST", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Ocular creado correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("CreateOcular", model);
        }

        [HttpGet]
        public IActionResult Edit(string tipo, int id)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            var json = _http.ObtenerBody(_http.EnviarSolicitud($"{UrlTipo(tipo)}/{id}", "GET", token: Token));
            return tipo.ToLower() switch
            {
                "montura" or "monturas" => View("EditMontura", JsonSerializer.Deserialize<MonturaViewModel>(json, _opts) ?? new()),
                "camara" or "camaras" => View("EditCamara", JsonSerializer.Deserialize<CamaraViewModel>(json, _opts) ?? new()),
                "ocular" or "oculares" => View("EditOcular", JsonSerializer.Deserialize<OcularViewModel>(json, _opts) ?? new()),
                _ => View("EditTelescopio", JsonSerializer.Deserialize<TelescopioViewModel>(json, _opts) ?? new())
            };
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditTelescopio(TelescopioViewModel model, string tipo = "telescopios")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("EditTelescopio", model);
            var dto = new { model.Marca, model.Modelo, model.Apertura, model.RelacionFocal, model.DistanciaFocal, model.Peso, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud($"{UrlTipo(tipo)}/{model.Id}", "PUT", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Telescopio actualizado correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("EditTelescopio", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditMontura(MonturaViewModel model, string tipo = "monturas")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("EditMontura", model);
            var dto = new { model.Marca, model.Modelo, model.TipoMontura, model.CargaUtil, model.EsGoTo, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud($"{UrlTipo(tipo)}/{model.Id}", "PUT", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Montura actualizada correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("EditMontura", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditCamara(CamaraViewModel model, string tipo = "camaras")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("EditCamara", model);
            var dto = new { model.Marca, model.Modelo, model.TipoSensor, model.Resolucion, model.TamanoPixel, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud($"{UrlTipo(tipo)}/{model.Id}", "PUT", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Cámara actualizada correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("EditCamara", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditOcular(OcularViewModel model, string tipo = "oculares")
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            ViewBag.Tipo = tipo.ToLower();
            if (!ModelState.IsValid) return View("EditOcular", model);
            var dto = new { model.Marca, model.Modelo, model.Diametro, model.AnguloVision, model.CantidadDisponible };
            var resp = _http.EnviarSolicitud($"{UrlTipo(tipo)}/{model.Id}", "PUT", dto, Token, throwOnError: false);
            if (resp.IsSuccessStatusCode) { TempData["Exito"] = "Ocular actualizado correctamente."; return RedirectToAction("Index", new { tipo }); }
            ModelState.AddModelError("", _http.ObtenerMensajeError(resp));
            return View("EditOcular", model);
        }

        [HttpGet]
        public IActionResult Disponibilidad(int id)
        {
            if (Token == null) return Json(new { error = "No autorizado" });
            var json = _http.ObtenerBody(_http.EnviarSolicitud($"api/telescopios/{id}/disponibilidad", "GET", token: Token));
            return Content(json, "application/json");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(string tipo, int id)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            var resp = _http.EnviarSolicitud($"{UrlTipo(tipo)}/{id}", "DELETE", token: Token);
            if (!resp.IsSuccessStatusCode) TempData["Error"] = _http.ObtenerMensajeError(resp);
            else TempData["Exito"] = "Equipo eliminado correctamente.";
            return RedirectToAction("Index", new { tipo });
        }
    }
}
