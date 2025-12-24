namespace AlquilerAuto.ViewModels
{
    /// <summary>
    /// DTO para exponer configuraciones financieras agrupadas
    /// </summary>
    public class ConfiguracionFinancieraDTO
    {
        public decimal MoraPorHora { get; set; }
        public decimal MoraPorDia { get; set; }
        public int TiempoGraciaMinutos { get; set; }
        public int KmRevisionPreventiva { get; set; }
        public bool PermitirReservasSimultaneas { get; set; }
        public int DiasAnticipacionReserva { get; set; }
    }
}
