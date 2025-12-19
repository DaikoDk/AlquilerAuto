using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;
namespace AlquilerAuto.Repositorio
{
    public interface IReserva: ICrud<Reserva>
    {
        string Cancelar(int idReserva);
        string Finalizar(int idReserva);
        List<Reserva> listarReservados();
        ReservaDetalleVM BuscarDetalle(int idReserva);
    }
}
