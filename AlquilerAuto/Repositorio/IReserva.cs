using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Repositorio
{
    public interface IReserva : ICrud<Reserva>
    {
        string IniciarAlquiler(int idReserva, string usuarioInicio);
        string Finalizar(int idReserva, int kilometrajeFin, string estadoEntrega, string observaciones, string usuarioFinalizacion);
        string Cancelar(int idReserva);
        bool ValidarDisponibilidad(int idAuto, DateTime fechaInicio, TimeSpan horaInicio, DateTime fechaFin, TimeSpan horaFin, int? idReservaExcluir = null);
        List<Reserva> listarReservados();
        ReservaDetalleVM BuscarDetalle(int idReserva);
    }
}
