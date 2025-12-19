using AlquilerAuto.DAO;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Service.ServiceImpl
{
    public class ReservaService : IReservaService
    {
        private readonly IReserva _reservaDAO;
        private readonly ICliente _clienteDAO;
        private readonly IAuto _autoDAO;

        public ReservaService(IReserva reservaDAO, ICliente clienteDAO, IAuto autoDAO)
        {
            _reservaDAO = reservaDAO;
            _clienteDAO = clienteDAO;
            _autoDAO = autoDAO;
        }

        public string AgregarReserva(ReservaVM vm)
        {

            // Conversión aquí mismo, NO en el Controller
            var reg = new Reserva
            {
                idCliente = vm.idCliente ?? 0,
                idAuto = vm.idAuto ?? 0,
                fechaInicio = vm.fechaInicio ?? DateTime.MinValue,
                fechaFin = vm.fechaFin ?? DateTime.MinValue
                // agrega otros campos si lo necesitas
            };

            // Validación de fechas
            if (reg.fechaFin < reg.fechaInicio)
                return "La fecha de fin no puede ser anterior a la fecha de inicio.";

            // Validar que el cliente NO tenga reserva activa
            var nombreCliente = _clienteDAO.buscar(reg.idCliente).nombreApe;
            var reservasCliente = _reservaDAO.Listado()
                .Where(r => r.cliente == nombreCliente && r.estado == "Reservado"); // Puedes extender el filtro de estado
            if (reservasCliente.Any())
                return "El cliente ya tiene una reserva activa.";

            // Validar auto disponible (por placa, modelo y estado)
            var auto = _autoDAO.buscar(reg.idAuto);
            if (auto.estado != "Disponible")
                return "El auto seleccionado no está disponible.";

            if (!AutoDisponible(reg.idAuto, reg.fechaInicio, reg.fechaFin))
                return "El auto seleccionado no está disponible para las fechas elegidas.";

            // Calcular total
            int dias = (reg.fechaFin - reg.fechaInicio).Days + 1;
            reg.total = auto.precioPorDia * dias;

            reg.estado = "Reservado";

            // Registrar reserva
            var mensaje = _reservaDAO.agregar(reg);
            return mensaje=="OK" ? "Reserva registrada correctamente.": mensaje;
        }

        public bool AutoDisponible(int idAuto, DateTime fechaInicio, DateTime fechaFin, int? idReservaEditar = null)
        {
            var reservas = _reservaDAO.Listado()
         .Where(r => r.idAuto == idAuto && r.estado == "Reservado");

            // Si se está editando una reserva, debe excluirse para no solaparse consigo misma
            if (idReservaEditar.HasValue)
                reservas = reservas.Where(r => r.idReserva != idReservaEditar.Value);

            foreach (var r in reservas)
            {
                // Si las fechas se solapan, el auto NO está disponible
                bool cruza = fechaInicio <= r.fechaFin && fechaFin >= r.fechaInicio;
                if (cruza)
                    return false;
            }
            return true;
        }

        public ReservaDetalleVM BuscarDetalleVM(int id)
        {
           return _reservaDAO.BuscarDetalle(id);
        }

        public Reserva BuscarReserva(int id)
        {return _reservaDAO.buscar(id);
        }

        public string CancelarReserva(int id)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null)
                return "La reserva no existe.";

            if (reserva.estado != "Reservado")
                return "Solo se pueden cancelar reservas en estado 'Reservado'.";

            var mensaje = _reservaDAO.Cancelar(id);

            return mensaje == "OK" ? "OK" : mensaje;
        }

        public string EliminarReserva(int id)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null)
                return "La reserva no existe.";

            // Solo permite eliminar si está Finalizado o Cancelado
            if (reserva.estado != "Finalizado" && reserva.estado != "Cancelado")
                return "Solo puedes eliminar reservas que estén en estado Finalizado o Cancelado.";

            return _reservaDAO.eliminar(new Reserva { idReserva = id });
        }

        public string FinalizarReserva(int id)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null)
                return "La reserva no existe.";

            // Solo finalizar si: Estado es "Reservado" o  FechaFin ha pasado o es hoy

            if (reserva.estado == "Reservado" && DateTime.Now.Date >= reserva.fechaFin.Date)
            {
                return _reservaDAO.Finalizar(id); // Actualiza estado a 'Finalizado'
            }
            else if (reserva.estado != "Reservado")
            {
                return "Solo puedes finalizar reservas que estén en estado 'Reservado'.";
            }
            else
            {
                return "No se puede finalizar aún. La fecha de fin de la reserva no ha llegado.";
            }
        }

        public List<Reserva> Listar()
        {
            // Obtiene todas las reservas del DAO
            var todas = _reservaDAO.Listado();
            // Filtra solo las que están “Reservado”
            return todas.Where(r => r.estado == "Reservado").ToList();
        }
    }

}
