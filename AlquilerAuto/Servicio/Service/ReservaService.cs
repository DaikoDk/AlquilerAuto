using AlquilerAuto.DAO;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Servicio.Service
{
    public class ReservaService : IReservaService
    {
        private readonly IReserva _reservaDAO;
        private readonly ICliente _clienteDAO;
        private readonly IAuto _autoDAO;
        private readonly IReparacion _reparacionDAO;
        private readonly IConfiguracionService _configuracionService;

        public ReservaService(
            IReserva reservaDAO, 
            ICliente clienteDAO, 
            IAuto autoDAO, 
            IReparacion reparacionDAO,
            IConfiguracionService configuracionService)
        {
            _reservaDAO = reservaDAO;
            _clienteDAO = clienteDAO;
            _autoDAO = autoDAO;
            _reparacionDAO = reparacionDAO;
            _configuracionService = configuracionService;
        }

        public string AgregarReserva(ReservaVM vm)
        {
            // Conversión del ViewModel a Modelo
            var reg = new Reserva
            {
                idCliente = vm.idCliente ?? 0,
                idAuto = vm.idAuto ?? 0,
                fechaInicio = vm.fechaInicio ?? DateTime.MinValue,
                horaInicio = vm.horaInicio ?? TimeSpan.Zero,
                fechaFin = vm.fechaFin ?? DateTime.MinValue,
                horaFin = vm.horaFin ?? TimeSpan.Zero
            };

            // Validación de fechas y horas
            var fechaHoraInicio = reg.fechaInicio.Add(reg.horaInicio);
            var fechaHoraFin = reg.fechaFin.Add(reg.horaFin);

            if (fechaHoraFin <= fechaHoraInicio)
                return "La fecha/hora de fin debe ser posterior a la fecha/hora de inicio.";

            // Obtener configuración de días de anticipación
            var diasAnticipacion = _configuracionService.GetValorInt("DIAS_ANTICIPACION_RESERVA");
            var fechaMinimaReserva = DateTime.Now.AddDays(diasAnticipacion);

            if (fechaHoraInicio < fechaMinimaReserva)
                return $"Las reservas deben hacerse con al menos {diasAnticipacion} día(s) de anticipación.";

            // Validar que el cliente NO tenga reserva activa (según configuración)
            var cliente = _clienteDAO.buscar(reg.idCliente);
            if (cliente.bloqueado)
                return "El cliente está bloqueado y no puede realizar reservas.";

            // Obtener configuración de reservas simultáneas
            var permitirSimultaneas = _configuracionService.GetValorBoolean("PERMITIR_RESERVAS_SIMULTANEAS");
            
            if (!permitirSimultaneas)
            {
                var reservasCliente = _reservaDAO.Listado()
                    .Where(r => r.idCliente == reg.idCliente && (r.estado == "Reservado" || r.estado == "Alquilado"));
                
                if (reservasCliente.Any())
                    return "El cliente ya tiene una reserva activa. No se permiten reservas simultáneas.";
            }

            // Validar auto disponible
            var auto = _autoDAO.buscar(reg.idAuto);
            if (auto == null || auto.idAuto == 0)
                return "El auto seleccionado no existe.";

            if (auto.estado != "Disponible")
                return "El auto seleccionado no está disponible.";

            // Validar disponibilidad de horario
            if (!_reservaDAO.ValidarDisponibilidad(reg.idAuto, reg.fechaInicio, reg.horaInicio, reg.fechaFin, reg.horaFin))
                return "El auto ya tiene una reserva en ese horario.";

            // Calcular total
            var duracionTotal = fechaHoraFin - fechaHoraInicio;
            
            if (duracionTotal.TotalHours < 24 && auto.precioPorHora.HasValue)
            {
                // Cobrar por horas
                var horas = Math.Ceiling(duracionTotal.TotalHours);
                reg.subtotal = auto.precioPorHora.Value * (decimal)horas;
            }
            else
            {
                // Cobrar por días
                var dias = Math.Ceiling(duracionTotal.TotalDays);
                reg.subtotal = auto.precioPorDia * (decimal)dias;
            }

            reg.total = reg.subtotal;
            reg.estado = "Reservado";
            reg.usuarioCreacion = "Sistema";

            // Registrar reserva
            var mensaje = _reservaDAO.agregar(reg);
            
            if (mensaje.Equals("OK", StringComparison.OrdinalIgnoreCase) || 
                mensaje.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return "Reserva registrada correctamente.";
            }
            
            return mensaje;
        }

        public ReservaDetalleVM BuscarDetalleVM(int id)
        {
            var detalle = _reservaDAO.BuscarDetalle(id);
            
            // Agregar costo de reparaciones si no está calculado
            if (detalle != null && detalle.IdReserva.HasValue)
            {
                var costoReparaciones = ObtenerTotalReparacionesCliente(detalle.IdReserva.Value);
                detalle.CostoReparaciones = costoReparaciones;
            }
            
            return detalle;
        }

        public decimal ObtenerTotalReparacionesCliente(int idReserva)
        {
            // ===== LÓGICA DE NEGOCIO ENCAPSULADA =====
            var reparaciones = _reparacionDAO.ListadoPorReserva(idReserva);
            
            var totalCliente = reparaciones
                .Where(r => r.responsable == "Cliente")
                .Sum(r => r.costo);

            return totalCliente;
        }

        public Reserva BuscarReserva(int id)
        {
            return _reservaDAO.buscar(id);
        }

        public string CancelarReserva(int id)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null || reserva.idReserva == 0)
                return "La reserva no existe.";

            if (reserva.estado != "Reservado" && reserva.estado != "Alquilado")
                return "Solo se pueden cancelar reservas en estado 'Reservado' o 'Alquilado'.";

            var mensaje = _reservaDAO.Cancelar(id);
            return mensaje == "OK" ? "OK" : mensaje;
        }

        public string EliminarReserva(int id)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null || reserva.idReserva == 0)
                return "La reserva no existe.";

            if (reserva.estado != "Finalizado" && reserva.estado != "Cancelado")
                return "Solo puedes eliminar reservas que estén en estado Finalizado o Cancelado.";

            return _reservaDAO.eliminar(new Reserva { idReserva = id });
        }

        public string IniciarAlquiler(int id, string usuario)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null || reserva.idReserva == 0)
                return "La reserva no existe.";

            if (reserva.estado != "Reservado")
                return "Solo se pueden iniciar reservas en estado 'Reservado'.";

            var mensaje = _reservaDAO.IniciarAlquiler(id, usuario);
            return mensaje == "OK" ? "Alquiler iniciado correctamente." : mensaje;
        }

        public string FinalizarReserva(int id, int kilometrajeFin, string estadoEntrega, string observaciones, string usuario)
        {
            var reserva = _reservaDAO.buscar(id);
            if (reserva == null || reserva.idReserva == 0)
                return "La reserva no existe.";

            if (reserva.estado != "Alquilado")
                return "Solo se pueden finalizar reservas en estado 'Alquilado'.";

            // Validar kilometraje
            if (reserva.kilometrajeInicio.HasValue && kilometrajeFin < reserva.kilometrajeInicio.Value)
                return "El kilometraje final no puede ser menor al inicial.";

            var mensaje = _reservaDAO.Finalizar(id, kilometrajeFin, estadoEntrega, observaciones ?? "", usuario);
            return mensaje == "OK" ? "Reserva finalizada correctamente." : mensaje;
        }

        public List<Reserva> Listar()
        {
            // Iniciar automáticamente reservas que ya pasaron su fecha/hora de inicio
            IniciarReservasAutomaticamente();
            
            return _reservaDAO.Listado().ToList();
        }

        public List<Reserva> ListarActivas()
        {
            // Iniciar automáticamente reservas que ya pasaron su fecha/hora de inicio
            IniciarReservasAutomaticamente();
            
            return _reservaDAO.Listado()
                .Where(r => r.estado == "Reservado" || r.estado == "Alquilado")
                .ToList();
        }

        private void IniciarReservasAutomaticamente()
        {
            // Obtener todas las reservas en estado "Reservado"
            var reservasReservadas = _reservaDAO.Listado()
                .Where(r => r.estado == "Reservado")
                .ToList();

            var ahora = DateTime.Now;

            foreach (var reserva in reservasReservadas)
            {
                // Calcular la fecha/hora de inicio
                var fechaHoraInicio = reserva.fechaInicio.Add(reserva.horaInicio);

                // Si ya pasó la fecha/hora de inicio, iniciar automáticamente
                if (ahora >= fechaHoraInicio)
                {
                    try
                    {
                        _reservaDAO.IniciarAlquiler(reserva.idReserva, "Sistema-Auto");
                    }
                    catch (Exception)
                    {
                        // Si falla, continuar con las demás reservas
                        continue;
                    }
                }
            }
        }
    }
}
