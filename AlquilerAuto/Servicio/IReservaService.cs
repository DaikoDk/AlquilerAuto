using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Servicio
{
    public interface IReservaService
    {  
        List<Reserva> Listar();
        List<Reserva> ListarActivas();
        string AgregarReserva(ReservaVM vm);
        string EliminarReserva(int id);
        string IniciarAlquiler(int id, string usuario);
        string CancelarReserva(int id);
        string FinalizarReserva(int id, int kilometrajeFin, string estadoEntrega, string observaciones, string usuario);
        Reserva BuscarReserva(int id);
        ReservaDetalleVM BuscarDetalleVM(int id);
        
        // Lógica de negocio para reparaciones
        decimal ObtenerTotalReparacionesCliente(int idReserva);
    }
}
