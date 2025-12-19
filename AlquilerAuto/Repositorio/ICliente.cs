using AlquilerAuto.Models;

namespace AlquilerAuto.Repositorio
{
    public interface ICliente: ICrud<Cliente>
    {
        bool existeDni(string dni);
        bool existeEmail(string email);
    }
}
