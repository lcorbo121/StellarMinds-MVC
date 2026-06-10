using Microsoft.AspNetCore.Mvc;
using StellarMinds.Models.Auth;
using System.Text.Json;
using WebApp.Services.Http;

namespace StellarMinds.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AuxiliarClienteHttp _http;

        public UsuariosController(AuxiliarClienteHttp http)
        {
            _http = http;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("token") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = _http.EnviarSolicitud(
                    "api/auth/login",
                    "POST",
                    new { username = model.UserName, password = model.Password }
                );

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(model);
                }

                var json = _http.ObtenerBody(response);
                var result = JsonSerializer.Deserialize<LoginResultDto>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                {
                    ModelState.AddModelError("", "Error al procesar la respuesta del servidor.");
                    return View(model);
                }

                HttpContext.Session.SetString("token", result.Token);
                HttpContext.Session.SetString("userName", result.Usuario.UserName);
                HttpContext.Session.SetString("rol", result.Usuario.Rol);
                HttpContext.Session.SetInt32("userId", result.Usuario.Id);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo conectar con el servidor. Verifique que la API esté en ejecución.");
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Usuarios");
        }
    }
}
