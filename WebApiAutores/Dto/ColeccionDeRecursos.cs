namespace WebApiAutores.Dto
{
    //va usar genericos <T>
    //TTotdo T debe eredar de recursso asegurar que autorDtoEredede recursos
    public class ColeccionDeRecursos<T>:Recurso where T : Recurso
    {
        public List<T> Valores { get; set; }
    }
}
