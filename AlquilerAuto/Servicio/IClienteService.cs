using AlquilerAuto.Models;

namespace AlquilerAuto.Service
{
    public interface IClienteService
    {
        IEnumerable<Cliente> Listar();
        Cliente Buscar(int id);
        string AgregarCliente(Cliente cliente);
        string ActualizarCliente(Cliente cliente);
        string EliminarCliente(int idCliente);
    }
}
