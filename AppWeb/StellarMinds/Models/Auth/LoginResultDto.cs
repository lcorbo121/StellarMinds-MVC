namespace StellarMinds.Models.Auth
{
    public class LoginResultDto
    {
        public string Token { get; set; } = "";
        public UsuarioAuthDto Usuario { get; set; } = new();
    }

    public class UsuarioAuthDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
