using WebApp.Services.Http;

namespace StellarMinds
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Sesiones
            builder.Services.AddSession();

            // Contenedor de inyeccion de dependencias

            // Definine el cliente HTTP para consumir la API
            builder.Services.AddHttpClient("Api", c => c.BaseAddress = new Uri("https://localhost:7158/"));

            // Registrar auxiliar http (síncrono simple)
            builder.Services.AddScoped<AuxiliarClienteHttp>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Usuarios}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
