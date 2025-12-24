using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Servicio.Service
{
    public class ReparacionService : IReparacionService
    {
        private readonly IReparacion _reparacionDAO;
        private readonly IReserva _reservaDAO;

        public ReparacionService(IReparacion reparacionDAO, IReserva reservaDAO)
        {
            _reparacionDAO = reparacionDAO;
            _reservaDAO = reservaDAO;
        }

        public IEnumerable<CatalogoReparacion> ListarCatalogo()
        {
            return _reparacionDAO.ListadoCatalogo();
        }

        public IEnumerable<Reparacion> ListarPorReserva(int idReserva)
        {
            return _reparacionDAO.ListadoPorReserva(idReserva);
        }

        public string CrearReparacion(ReparacionCreateDTO dto, string usuarioReporte)
        {
            // ===== VALIDACIONES DE NEGOCIO =====

            // 1. Validar costo no negativo
            if (dto.Costo < 0)
                return "El costo no puede ser negativo.";

            // 2. Validar descripción no vacía
            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return "La descripción es obligatoria.";

            // 3. Validar que la reserva exista
            var reserva = _reservaDAO.buscar(dto.IdReserva);
            if (reserva == null || reserva.idReserva == 0)
                return "La reserva especificada no existe.";

            // 4. Validar que la reserva NO esté cancelada
            if (reserva.estado == "Cancelado")
                return "No se pueden registrar reparaciones en reservas canceladas.";

            // 5. Validar responsable válido
            var responsablesValidos = new[] { "Cliente", "Empresa", "Tercero" };
            if (!responsablesValidos.Contains(dto.Responsable))
                return "Responsable no válido. Debe ser Cliente, Empresa o Tercero.";

            // 6. Validar estado válido
            var estadosValidos = new[] { "Pendiente", "En proceso", "Completada" };
            if (!estadosValidos.Contains(dto.EstadoReparacion))
                return "Estado no válido. Debe ser Pendiente, En proceso o Completada.";

            // ===== MAPEO DE DTO A ENTIDAD =====
            var reparacion = new Reparacion
            {
                idReserva = dto.IdReserva,
                idAuto = dto.IdAuto,
                idCatalogoReparacion = dto.IdCatalogoReparacion,
                descripcion = dto.Descripcion,
                costo = dto.Costo,
                responsable = dto.Responsable,
                estado = dto.EstadoReparacion, // ? Usar el estado del DTO
                fechaReporte = DateTime.Now,
                usuarioReporte = usuarioReporte
            };

            // ===== GUARDAR EN BASE DE DATOS =====
            string resultado = _reparacionDAO.agregar(reparacion);

            if (resultado == "OK")
                return "Reparación registrada exitosamente.";

            return $"Error al registrar la reparación: {resultado}";
        }

        public string AgregarReparacion(Reparacion reparacion)
        {
            // Método legacy - mantener compatibilidad
            if (reparacion.costo < 0)
                return "El costo no puede ser negativo.";

            if (string.IsNullOrEmpty(reparacion.descripcion))
                return "La descripción es obligatoria.";

            string resultado = _reparacionDAO.agregar(reparacion);

            if (resultado == "OK")
                return "Reparación registrada exitosamente.";

            return resultado;
        }

        public string ActualizarEstado(int idReparacion, string estado, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            // ===== VALIDACIÓN DE NEGOCIO =====
            var estadosValidos = new[] { "Pendiente", "En proceso", "Completada", "Cancelada" };
            if (!estadosValidos.Contains(estado))
                return "Estado no válido.";

            string resultado = _reparacionDAO.ActualizarEstado(idReparacion, estado, fechaInicio, fechaFin);

            if (resultado == "OK")
                return "Estado actualizado exitosamente.";

            return resultado;
        }

        public string ActualizarReparacion(Reparacion reparacion)
        {
            // ===== VALIDACIONES DE NEGOCIO =====
            if (reparacion.costo < 0)
                return "El costo no puede ser negativo.";

            var estadosValidos = new[] { "Pendiente", "En proceso", "Completada", "Cancelada" };
            if (!estadosValidos.Contains(reparacion.estado))
                return "Estado no válido.";

            string resultado = _reparacionDAO.actualizar(reparacion);

            if (resultado == "OK")
                return "Reparación actualizada exitosamente.";

            return resultado;
        }

        public decimal ObtenerTotalReparacionesCliente(int idReserva)
        {
            // ===== LÓGICA DE CÁLCULO ENCAPSULADA =====
            var reparaciones = _reparacionDAO.ListadoPorReserva(idReserva);
            
            var totalCliente = reparaciones
                .Where(r => r.responsable == "Cliente")
                .Sum(r => r.costo);

            return totalCliente;
        }

        public ReparacionCreateDTO? PrepararFormularioCreacion(int idReserva)
        {
            // ===== PREPARAR DATOS PARA LA VISTA =====
            var reservaDetalle = _reservaDAO.BuscarDetalle(idReserva);
            
            if (reservaDetalle == null || reservaDetalle.IdReserva == null)
                return null;

            // Obtener la reserva completa para tener el idAuto
            var reserva = _reservaDAO.buscar(idReserva);
            
            if (reserva == null || reserva.idAuto == 0)
                return null;

            return new ReparacionCreateDTO
            {
                IdReserva = idReserva,
                IdAuto = reserva.idAuto,
                NombreCliente = reservaDetalle.nombreCliente,
                PlacaAuto = reservaDetalle.placaAuto,
                ModeloAuto = reservaDetalle.modeloAuto,
                EstadoReserva = reservaDetalle.Estado,
                Responsable = "Cliente"
            };
        }
    }
}
