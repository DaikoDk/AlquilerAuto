using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;
namespace AlquilerAuto.Repositorio
{
    public interface IReserva: ICrud<Reserva>
    {
        string Cancelar(int idReserva);
        string Finalizar(int idReserva);

        ReservaDetalleVM BuscarDetalle(int idReserva);

    }
}
