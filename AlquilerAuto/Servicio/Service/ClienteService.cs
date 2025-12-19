using AlquilerAuto.Repositorio;
using AlquilerAuto.Models;
using AlquilerAuto.DAO;

namespace AlquilerAuto.Service.ServiceImpl
{
    public class ClienteService : IClienteService
    {
        private readonly ICliente _clienteDAO;
        private readonly IReserva _reservaDAO;
        public ClienteService(ICliente clienteDAO, IReserva reservaDAO)
        {
            _clienteDAO = clienteDAO;
            _reservaDAO = reservaDAO;
        }

        public string AgregarCliente(Cliente reg)
        {
            if (string.IsNullOrWhiteSpace(reg.nombreApe) ||
                string.IsNullOrWhiteSpace(reg.dni) ||
                string.IsNullOrWhiteSpace(reg.telefono) ||
                string.IsNullOrWhiteSpace(reg.email))
                {
                return "Todos los campos obligatorios deben estar llenos.";
                }

            if (_clienteDAO.existeDni(reg.dni)) {
                return "El DNI ingresado ya está registrado a otro cliente";
            }

            if (_clienteDAO.existeEmail(reg.email)) {
                return "El email ingresado ya está registrado a otro cliente";
            }

            string resultado = _clienteDAO.agregar(reg);

            if (resultado == "OK") {
                return "Cliente registrado exitosamente."; }
            else if (!string.IsNullOrWhiteSpace(resultado)) {
                return $"Ocurrió un error al registrar: {resultado}"; }
            else
                return $"Ocurrió un error inesperado al registrar el cliente";
        }

        public string ActualizarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.nombreApe) ||
                string.IsNullOrWhiteSpace(cliente.dni) ||
                string.IsNullOrWhiteSpace(cliente.telefono) ||
                string.IsNullOrWhiteSpace(cliente.email))
            {
                return "Todos los campos obligatorios deben estar llenos.";
            }

            var duplicadoDni = _clienteDAO.Listado().Any(x => x.dni == cliente.dni && x.idCliente != cliente.idCliente);
            if (duplicadoDni)
            {
                return "El DNI ingresado ya está registrado a otro cliente."; }

            var duplicadoEmail = _clienteDAO.Listado().Any(x => x.email == cliente.email && x.idCliente != cliente.idCliente);
            if (duplicadoEmail)
            {
                return "El Email ingresado ya está registrado a otro cliente.";}

            string resultado = _clienteDAO.actualizar(cliente);

            //Mejor comparación por exito
            if (resultado.Trim().ToUpper() == "OK")
            {
                return "Datos de cliente actualizados correctamente."; }
            else if (!string.IsNullOrWhiteSpace(resultado))
            {
                return $"Ocurrió un error al actualizar: {resultado}"; }
            else
                return $"Ocurrió un error inesperado al actualizar el cliente.";
        }

        public string EliminarCliente(int idCliente)
        {
            /* Ejemplo de validación de reservas (si tienes el DAO correspondiente):
            if (_reservaDAO.ClienteTieneReservas(idCliente))
                return "No se puede eliminar este cliente porque tiene reservas registradas.";

            string resultado = _clienteDAO.eliminar(new Cliente { idCliente = idCliente });

            if (resultado.Trim().ToUpper() == "OK")
                return "Cliente eliminado correctamente.";
            else if (!string.IsNullOrWhiteSpace(resultado))
                return $"Ocurrió un error al eliminar: {resultado}";
            else
                return "Ocurrió un error inesperado al eliminar el cliente.";
            */
            return "Funcionalidad de eliminación no implementada.";
        }

        public IEnumerable<Cliente> Listar()
        {
            return _clienteDAO.Listado();
        }

        public Cliente Buscar(int id)
        {
            var cliente = _clienteDAO.buscar(id);
            if (cliente == null || cliente.idCliente == 0)
                throw new Exception("No se encontró un cliente con el ID proporcionado.");
            return cliente;
        }
    }
}
