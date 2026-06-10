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

        public HttpResponseMessage EnviarSolicitud(string relativeUrl, string verbo, object? body = null, string? token = null, bool throwOnError = true)
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
            
        // TERMINAR lcorbo
        // REVISAR REPORTAR ERROR EN RESPUESTA DE API, NO SE MUESTRA MENSAJE DE ERROR EN VISTA
     
            if (throwOnError) resp.EnsureSuccessStatusCode();
            return resp;
        }
        // ESTA LINEA ESTA MAL EN EL CONCEPTO SOLID.


        public string ObtenerBody(HttpResponseMessage respuesta)
        {
            return respuesta.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public T? EnviarYDeserializar<T>(string relativeUrl, string verbo, object? body = null, string? token = null, bool throwOnError = true)
        {
            var resp = EnviarSolicitud(relativeUrl, verbo, body, token, throwOnError);
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
