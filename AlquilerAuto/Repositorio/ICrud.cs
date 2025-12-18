namespace AlquilerAuto.Repositorio
{
    public interface ICrud<T> where T : class
    {
        IEnumerable<T> Listado();
        T buscar(int codigo);
        string agregar(T reg);
        string actualizar(T reg);
        string eliminar(T reg);
    }
}
