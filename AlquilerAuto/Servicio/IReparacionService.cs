using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Servicio
{
    public interface IReparacionService
    {
        IEnumerable<CatalogoReparacion> ListarCatalogo();
        IEnumerable<Reparacion> ListarPorReserva(int idReserva);
        
        // Método principal con DTO
        string CrearReparacion(ReparacionCreateDTO dto, string usuarioReporte);
        
        // Método para actualizar (editar)
        string ActualizarReparacion(Reparacion reparacion);
        
        // Método legacy (mantener compatibilidad)
        string AgregarReparacion(Reparacion reparacion);
        
        string ActualizarEstado(int idReparacion, string estado, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        
        // Métodos de lógica de negocio
        decimal ObtenerTotalReparacionesCliente(int idReserva);
        ReparacionCreateDTO? PrepararFormularioCreacion(int idReserva);
    }
}
