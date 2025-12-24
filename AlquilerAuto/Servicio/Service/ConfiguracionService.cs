using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Servicio.Service
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracion _configuracionDAO;

        public ConfiguracionService(IConfiguracion configuracionDAO)
        {
            _configuracionDAO = configuracionDAO;
        }

        // ===== MÉTODOS GENÉRICOS =====

        public string GetValor(string clave)
        {
            return _configuracionDAO.ObtenerValor(clave);
        }

        public int GetValorInt(string clave)
        {
            return _configuracionDAO.ObtenerValorEntero(clave);
        }

        public decimal GetValorDecimal(string clave)
        {
            return _configuracionDAO.ObtenerValorDecimal(clave);
        }

        public bool GetValorBoolean(string clave)
        {
            return _configuracionDAO.ObtenerValorBooleano(clave);
        }

        // ===== MÉTODO HELPER ESPECÍFICO =====

        /// <summary>
        /// Obtiene todas las configuraciones financieras en un solo DTO
        /// Esto evita múltiples llamadas a la BD desde otros servicios
        /// </summary>
        public ConfiguracionFinancieraDTO ObtenerConfiguracionFinanciera()
        {
            return new ConfiguracionFinancieraDTO
            {
                MoraPorHora = GetValorDecimal("MORA_POR_HORA"),
                MoraPorDia = GetValorDecimal("MORA_POR_DIA"),
                TiempoGraciaMinutos = GetValorInt("TIEMPO_GRACIA_MINUTOS"),
                KmRevisionPreventiva = GetValorInt("KM_REVISION_PREVENTIVA"),
                PermitirReservasSimultaneas = GetValorBoolean("PERMITIR_RESERVAS_SIMULTANEAS"),
                DiasAnticipacionReserva = GetValorInt("DIAS_ANTICIPACION_RESERVA")
            };
        }

        // ===== MÉTODOS DE ADMINISTRACIÓN =====

        public IEnumerable<Configuracion> ListarTodas()
        {
            return _configuracionDAO.ListarTodas();
        }

        public string Actualizar(string clave, string valor)
        {
            // ===== VALIDACIONES DE NEGOCIO =====

            if (string.IsNullOrWhiteSpace(clave))
                return "La clave no puede estar vacía.";

            if (string.IsNullOrWhiteSpace(valor))
                return "El valor no puede estar vacío.";

            // Validar formato según el tipo esperado
            switch (clave)
            {
                case "MORA_POR_HORA":
                case "MORA_POR_DIA":
                    if (!decimal.TryParse(valor, out decimal valorDecimal) || valorDecimal < 0)
                        return "El valor debe ser un número decimal positivo.";
                    break;

                case "TIEMPO_GRACIA_MINUTOS":
                case "KM_REVISION_PREVENTIVA":
                case "DIAS_ANTICIPACION_RESERVA":
                    if (!int.TryParse(valor, out int valorInt) || valorInt < 0)
                        return "El valor debe ser un número entero positivo.";
                    break;

                case "PERMITIR_RESERVAS_SIMULTANEAS":
                    if (valor != "0" && valor != "1" && 
                        !valor.Equals("true", StringComparison.OrdinalIgnoreCase) && 
                        !valor.Equals("false", StringComparison.OrdinalIgnoreCase))
                        return "El valor debe ser 0, 1, true o false.";
                    break;
            }

            string resultado = _configuracionDAO.Actualizar(clave, valor);

            if (resultado == "OK")
                return "Configuración actualizada exitosamente.";

            return resultado;
        }
    }
}
