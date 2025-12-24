using AlquilerAuto.Models;

namespace AlquilerAuto.Repositorio
{
    public interface IConfiguracion
    {
        string ObtenerValor(string clave);
        int ObtenerValorEntero(string clave);
        decimal ObtenerValorDecimal(string clave);
        bool ObtenerValorBooleano(string clave);
        string Actualizar(string clave, string valor);
        IEnumerable<Configuracion> ListarTodas();
    }
}
