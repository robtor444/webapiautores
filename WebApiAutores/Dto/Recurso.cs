namespace WebApiAutores.Dto
{
    public class Recurso
    {

        //es parte del hateoas

        public List<DatoHateOAS> Enlaces { get; set; }=new List<DatoHateOAS>();
    }
}
