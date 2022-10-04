namespace WebApiAutores.Dto
{
    public class DatoHateOAS
    {

        //para heateoas
        //para que no podamos modificarlo ponemos private
        public string Enlace { get;private set; }
        public string Descripcion{ get;private set; }

        public string Metodo{ get;private set; }

        public DatoHateOAS(string enlace, string descripcion, string metodo)
        {
            Enlace=enlace;
            Descripcion=descripcion;
            Metodo=metodo;
        }
    }
}
