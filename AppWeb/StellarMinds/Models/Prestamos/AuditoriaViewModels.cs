using StellarMinds.Models.Usuarios;

namespace StellarMinds.Models.Prestamos
{
    public class AuditoriaItem
    {
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public string? CoordinadorNombre { get; set; }
        public string? Accion { get; set; }
        public DateTime FechaAccion { get; set; }
    }

    // RF11: un préstamo en el listado auditable = el préstamo + el coordinador que lo dio de alta.
    public class PrestamoAuditableItem
    {
        public PrestamoListaItem Prestamo { get; set; } = new();
        public string? CoordinadorNombre { get; set; }
    }

    // RF11: listado de préstamos filtrable por el coordinador que lo realizó.
    public class AuditoriaViewModel
    {
        public List<PrestamoAuditableItem> Prestamos { get; set; } = new();
        public List<UsuarioListaItem> Coordinadores { get; set; } = new();
        public int? CoordinadorSeleccionado { get; set; }
    }

    // RF11: vista de auditoría de UN préstamo (sus acciones) con link al detalle.
    public class AuditoriaPrestamoViewModel
    {
        public int PrestamoId { get; set; }
        public List<AuditoriaItem> Auditoria { get; set; } = new();
    }
}
