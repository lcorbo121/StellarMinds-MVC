using System.ComponentModel.DataAnnotations;

namespace StellarMinds.Models.Socios
{
    public class SocioViewModel
    {
        [Required, Display(Name = "Nombre completo")] public string FullName { get; set; } = "";
        [Required, EmailAddress, Display(Name = "Email")] public string Email { get; set; } = "";
        [Required, Display(Name = "Calle")] public string Calle { get; set; } = "";
        [Required, Display(Name = "Número de puerta"), Range(1, 99999)] public int NumeroPuerta { get; set; }
        [Display(Name = "Apartamento"), Range(0, 9999)] public int Apartamento { get; set; }
        [Required, Display(Name = "Contraseña")] public string Password { get; set; } = "";
        [Required, Display(Name = "Teléfono")] public string PhoneNumber { get; set; } = "";
        [Required, Display(Name = "Nombre de usuario")] public string UserName { get; set; } = "";
        [Required, Display(Name = "Rol")] public string RolNombre { get; set; } = "Socio";
    }
}
