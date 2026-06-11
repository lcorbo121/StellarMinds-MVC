namespace StellarMinds.Models.Usuarios
{
    // Item tipado para listados/detalle de usuarios (lo que devuelve la WebAPI).
    public class UsuarioListaItem
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = "";
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
    }
}
