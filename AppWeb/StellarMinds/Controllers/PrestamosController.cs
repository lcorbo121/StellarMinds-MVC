using Microsoft.AspNetCore.Mvc;
using StellarMinds.Models.Equipos;
using StellarMinds.Models.Prestamos;
using StellarMinds.Models.Usuarios;
using System.Text.Json;
using WebApp.Services.Http;

namespace StellarMinds.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly AuxiliarClienteHttp _http;
        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        public PrestamosController(AuxiliarClienteHttp http)
        {
            _http = http;
        }

        private string? Token => HttpContext.Session.GetString("token");
        private string? Rol => HttpContext.Session.GetString("rol");
        private int UserId => HttpContext.Session.GetInt32("userId") ?? 0;

        [HttpGet]
        public IActionResult Create()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Coordinador" && Rol != "Administrador") return Forbid();
            var socios = _http.EnviarYDeserializar<List<JsonElement>>("api/usuarios", "GET", token: Token) ?? [];
            var telescopios = _http.EnviarYDeserializar<List<JsonElement>>("api/telescopios", "GET", token: Token) ?? [];
            var monturas = _http.EnviarYDeserializar<List<JsonElement>>("api/monturas", "GET", token: Token) ?? [];
            var camaras = _http.EnviarYDeserializar<List<JsonElement>>("api/camaras", "GET", token: Token) ?? [];
            var oculares = _http.EnviarYDeserializar<List<JsonElement>>("api/oculares", "GET", token: Token) ?? [];
            ViewBag.SociosJson = JsonSerializer.Serialize(socios);
            ViewBag.TelescopiosJson = JsonSerializer.Serialize(telescopios);
            ViewBag.MonturasJson = JsonSerializer.Serialize(monturas);
            ViewBag.CamarasJson = JsonSerializer.Serialize(camaras);
            ViewBag.OcularesJson = JsonSerializer.Serialize(oculares);
            return View(new PrestamoViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(PrestamoViewModel model)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Coordinador" && Rol != "Administrador") return Forbid();
            if (!ModelState.IsValid) { TempData["Error"] = "Datos del formulario inválidos."; return RedirectToAction("Create"); }
            if (!model.CamaraId.HasValue && !model.OcularId.HasValue) { TempData["Error"] = "Debe seleccionar al menos una Cámara o un Ocular."; return RedirectToAction("Create"); }
            if (model.CamaraId.HasValue && model.OcularId.HasValue) { TempData["Error"] = "No puede seleccionar Cámara y Ocular a la vez: elija solo uno."; return RedirectToAction("Create"); }
            var dto = new { model.UsuarioId, model.TelescopioId, model.MonturaId, model.CamaraId, model.OcularId, FechaInicio = model.FechaInicio.ToString("yyyy-MM-dd"), FechaFin = model.FechaFin.ToString("yyyy-MM-dd") };
            var resp = _http.EnviarSolicitud("api/prestamos", "POST", dto, Token);
            if (resp.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Préstamo registrado correctamente.";
                // La API devuelve el id del préstamo creado: redirigimos a su detalle.
                var nuevoId = 0;
                try { using var doc = JsonDocument.Parse(_http.ObtenerBody(resp)); if (doc.RootElement.TryGetProperty("id", out var idEl)) nuevoId = idEl.GetInt32(); } catch { }
                return nuevoId > 0
                    ? RedirectToAction("DetallePrestamo", new { prestamoId = nuevoId })
                    : RedirectToAction("Create");
            }
            TempData["Error"] = _http.ObtenerMensajeError(resp);
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Devolucion()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Coordinador" && Rol != "Administrador") return Forbid();
            var socios = _http.EnviarYDeserializar<List<UsuarioListaItem>>("api/usuarios", "GET", token: Token) ?? [];
            return View(socios);
        }

        [HttpGet]
        public IActionResult PrestamosEnPrestamo(int socioId)
        {
            if (Token == null) return Json(new { error = "No autorizado" });
            var prestamos = _http.EnviarYDeserializar<List<PrestamoListaItem>>($"api/prestamos/vigentes-socio/{socioId}", "GET", token: Token) ?? [];
            return Json(prestamos);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Devolucion(int prestamoId)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Coordinador" && Rol != "Administrador") return Forbid();
            var resp = _http.EnviarSolicitud($"api/prestamos/{prestamoId}/devolucion", "PUT", token: Token);
            if (resp.IsSuccessStatusCode) TempData["Exito"] = "Devolución registrada correctamente.";
            else TempData["Error"] = _http.ObtenerMensajeError(resp);
            return RedirectToAction("Devolucion");
        }

        [HttpGet]
        public IActionResult MisPrestamos(int mes = 0, int anio = 0)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Socio" && Rol != "Administrador") return Forbid();
            mes = mes == 0 ? DateTime.Today.Month : mes;
            anio = anio == 0 ? DateTime.Today.Year : anio;
            var filtro = new FiltroFechaViewModel { Mes = mes, Anio = anio };
            var prestamos = _http.EnviarYDeserializar<List<PrestamoListaItem>>($"api/prestamos/mios?mes={mes}&anio={anio}", "GET", token: Token) ?? [];
            ViewBag.Prestamos = prestamos;
            if (!prestamos.Any()) ViewBag.SinResultados = $"No hay préstamos para {mes}/{anio}.";
            return View(filtro);
        }

        [HttpGet]
        public IActionResult TodosPrestamos()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Coordinador" && Rol != "Administrador") return Forbid();
            var prestamos = _http.EnviarYDeserializar<List<PrestamoListaItem>>("api/prestamos/todos", "GET", token: Token) ?? [];
            return View(prestamos);
        }

        [HttpGet]
        public IActionResult SociosPorTelescopio()
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador" && Rol != "Coordinador") return Forbid();
            var telescopios = _http.EnviarYDeserializar<List<TelescopioListaItem>>("api/telescopios", "GET", token: Token) ?? [];
            return View(telescopios);
        }

        [HttpGet]
        public IActionResult SociosDe(int telescopioId)
        {
            if (Token == null) return Json(new { error = "No autorizado" });
            var socios = _http.EnviarYDeserializar<List<UsuarioListaItem>>($"api/prestamos/socios-por-telescopio/{telescopioId}", "GET", token: Token) ?? [];
            return Json(socios);
        }

        [HttpGet]
        public IActionResult Auditoria(int? coordinadorId)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            // RF11: listado de PRÉSTAMOS filtrable por el coordinador que los dio de alta.
            var todos = _http.EnviarYDeserializar<List<PrestamoListaItem>>("api/prestamos/todos", "GET", token: Token) ?? [];
            var auditUrl = coordinadorId.HasValue ? $"api/prestamos/auditoria?coordinadorId={coordinadorId}" : "api/prestamos/auditoria";
            var audits = _http.EnviarYDeserializar<List<AuditoriaItem>>(auditUrl, "GET", token: Token) ?? [];
            var coordinadores = _http.EnviarYDeserializar<List<UsuarioListaItem>>("api/usuarios/todos", "GET", token: Token) ?? [];

            // Coordinador que dio de alta cada préstamo (acción "PRESTAMO").
            var coordPorPrestamo = audits
                .Where(a => a.Accion == "PRESTAMO")
                .GroupBy(a => a.PrestamoId)
                .ToDictionary(g => g.Key, g => g.First().CoordinadorNombre);

            // Con filtro: solo los préstamos cuya alta fue del coordinador seleccionado.
            IEnumerable<PrestamoListaItem> filtrados = coordinadorId.HasValue
                ? todos.Where(p => coordPorPrestamo.ContainsKey(p.Id))
                : todos;

            var prestamos = filtrados
                .Select(p => new PrestamoAuditableItem
                {
                    Prestamo = p,
                    CoordinadorNombre = coordPorPrestamo.TryGetValue(p.Id, out var c) ? c : "—"
                })
                .ToList();

            var vm = new AuditoriaViewModel
            {
                Prestamos = prestamos,
                Coordinadores = coordinadores,
                CoordinadorSeleccionado = coordinadorId
            };
            return View(vm);
        }

        // RF11: vista de auditoría de un préstamo (sus acciones), con link al detalle.
        [HttpGet]
        public IActionResult AuditoriaPrestamo(int prestamoId)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador") return Forbid();
            var auditoria = _http.EnviarYDeserializar<List<AuditoriaItem>>($"api/prestamos/{prestamoId}/auditoria", "GET", token: Token) ?? [];
            var vm = new AuditoriaPrestamoViewModel { PrestamoId = prestamoId, Auditoria = auditoria };
            return View(vm);
        }

        [HttpGet]
        public IActionResult DetallePrestamo(int prestamoId)
        {
            if (Token == null) return RedirectToAction("Index", "Usuarios");
            if (Rol != "Administrador" && Rol != "Coordinador") return Forbid();
            var prestamo = _http.EnviarYDeserializar<PrestamoListaItem>($"api/prestamos/{prestamoId}", "GET", token: Token);
            if (prestamo == null) { TempData["Error"] = "No se encontró el préstamo."; return RedirectToAction("TodosPrestamos"); }
            return View(prestamo);
        }
    }
}
