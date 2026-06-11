namespace StellarMinds.Models.Equipos
{
    // Item tipado para el desplegable de telescopios (RF09).
    public class TelescopioListaItem
    {
        public int Id { get; set; }
        public string Marca { get; set; } = "";
        public string Modelo { get; set; } = "";
    }
}
