using Microsoft.AspNetCore.HttpOverrides;
using WebApp.Services.Http;

namespace StellarMinds
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7158/";
            builder.Services.AddHttpClient("Api", c => c.BaseAddress = new Uri(apiBaseUrl));
            builder.Services.AddScoped<AuxiliarClienteHttp>();

            var app = builder.Build();

            // Permite que el app conozca el esquema real cuando está detrás del proxy de SOMEE/IIS
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Usuarios}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
