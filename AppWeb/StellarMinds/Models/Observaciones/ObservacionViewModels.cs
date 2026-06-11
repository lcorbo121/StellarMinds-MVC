namespace StellarMinds.Models.Observaciones
{
    // Item tipado para listados/detalle de observaciones.
    public class ObservacionItem
    {
        public int Id { get; set; }
        public string? FechaObservacion { get; set; }
        public string? Socio { get; set; }
        public string? ObjetoCeleste { get; set; }
        public string? TipoObjeto { get; set; }
        public string? Telescopio { get; set; }
        public string? IndicadorIA { get; set; }
        public string? DetalleIA { get; set; }
        public string? Notas { get; set; }
    }

    // Item del ranking de objetos celestes (RF10).
    public class RankingItem
    {
        public int Posicion { get; set; }
        public string? Nombre { get; set; }
        public string? Tipo { get; set; }
        public int CantidadObservaciones { get; set; }
    }
}
