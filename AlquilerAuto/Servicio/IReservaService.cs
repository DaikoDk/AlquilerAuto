using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Service
{
    public interface IReservaService
    {  
        List<Reserva> Listar(); 
        string AgregarReserva(ReservaVM vm);
        string EliminarReserva(int id);    
        string CancelarReserva(int id);    
        string FinalizarReserva(int id);  
        bool AutoDisponible(int idAuto, DateTime fechaInicio, DateTime fechaFin, int? idReservaEditar = null);
        Reserva BuscarReserva(int id);
        ReservaDetalleVM BuscarDetalleVM(int id);
    }
}
