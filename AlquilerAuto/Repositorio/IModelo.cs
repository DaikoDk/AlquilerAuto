using AlquilerAuto.Models;

namespace AlquilerAuto.Repositorio
{
    public interface IModelo : ICrud<Modelo>
    {
        IEnumerable<Modelo> ListadoPorMarca(int idMarca);
    }
}
