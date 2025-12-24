using AlquilerAuto.Models;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Servicio
{
    public interface IConfiguracionService
    {
        // Métodos genéricos para obtener valores
        string GetValor(string clave);
        int GetValorInt(string clave);
        decimal GetValorDecimal(string clave);
        bool GetValorBoolean(string clave);

        // Método específico para obtener configuración financiera agrupada
        ConfiguracionFinancieraDTO ObtenerConfiguracionFinanciera();

        // CRUD para administración
        IEnumerable<Configuracion> ListarTodas();
        string Actualizar(string clave, string valor);
    }
}
