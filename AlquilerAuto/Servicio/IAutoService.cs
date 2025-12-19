using AlquilerAuto.Models;

namespace AlquilerAuto.Service
{
    public interface IAutoService
    {
        IEnumerable<Auto> Listar();
        Auto Buscar(int id);
        string Agregar(Auto reg);
        string Actualizar(Auto reg);
        string Eliminar(int id);
        bool ExistePlaca(string placa, int? idAuto = null);
        IEnumerable<Auto> ListarDisponible();
    }
}
