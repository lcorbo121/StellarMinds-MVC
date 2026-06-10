using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebApp.Services.Http
{
    public class AuxiliarClienteHttp
    {
        private readonly IHttpClientFactory _factory;

        public AuxiliarClienteHttp(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public HttpResponseMessage EnviarSolicitud(string relativeUrl, string verbo, object? body = null, string? token = null)
        {
            var client = _factory.CreateClient("Api");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp = verbo.ToUpper() switch
            {
                "GET" => client.GetAsync(relativeUrl).GetAwaiter().GetResult(),
                "POST" => client.PostAsync(relativeUrl, CreateJsonContent(body)).GetAwaiter().GetResult(),
                "PUT" => client.PutAsync(relativeUrl, CreateJsonContent(body)).GetAwaiter().GetResult(),
                "DELETE" => client.DeleteAsync(relativeUrl).GetAwaiter().GetResult(),
                _ => throw new ArgumentException("Verbo no soportado", nameof(verbo))
            };

            // El helper solo transporta: NO decide la política de errores HTTP.
            // Cada caller inspecciona resp.IsSuccessStatusCode y actúa en consecuencia
            // (mostrar el mensaje con ObtenerMensajeError, devolver default, etc.).
            return resp;
        }

        public string ObtenerBody(HttpResponseMessage respuesta)
        {
            return respuesta.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        // Extrae el mensaje de error que devuelve la API en formato { "error": "..." }.
        // Si el body viene vacío o no se puede parsear, devuelve un mensaje genérico,
        // de modo que la vista siempre tenga algo legible para mostrar.
        public string ObtenerMensajeError(HttpResponseMessage respuesta)
        {
            const string generico = "Ocurrió un error inesperado.";
            try
            {
                var json = ObtenerBody(respuesta);
                if (string.IsNullOrWhiteSpace(json)) return generico;

                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty("error", out var err) &&
                    err.ValueKind == JsonValueKind.String)
                {
                    var msg = err.GetString();
                    return string.IsNullOrWhiteSpace(msg) ? generico : msg;
                }
                return generico;
            }
            catch
            {
                return generico;
            }
        }

        public T? EnviarYDeserializar<T>(string relativeUrl, string verbo, object? body = null, string? token = null)
        {
            var resp = EnviarSolicitud(relativeUrl, verbo, body, token);
            if (!resp.IsSuccessStatusCode) return default;
            var json = ObtenerBody(resp);
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return JsonSerializer.Deserialize<T>(json, opts);
        }

        private static HttpContent? CreateJsonContent(object? obj)
        {
            if (obj == null)
                return null;
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
