namespace WebApiAutores
{
    public interface IServicio
    {
        Guid ObtenerScope();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }

    public class ServicioA : IServicio
    {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScope servicioScope;
        private readonly ServicioSingleton servicioSingleton;

        public ServicioA(ILogger<ServicioA> logger
            ,ServicioTransient servicioTransient
            ,ServicioScope servicioScope
            ,ServicioSingleton servicioSingleton
            )
        {
            this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioScope = servicioScope;
            this.servicioSingleton = servicioSingleton;
        }

        public Guid ObtenerTransient()
        {
            return servicioTransient.guid;
        }
        public Guid ObtenerScope()
        {
            return servicioScope.guid;
        }
        public Guid ObtenerSingleton()
        {
            return servicioSingleton.guid;
        }
        public void RealizarTarea()
        {

        }
    }

    public class ServicioB : IServicio
    {
        public Guid ObtenerScope()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {

        }
    }

    public class ServicioTransient
    {
    
        public Guid guid=Guid.NewGuid();
    }

    public class ServicioScope
    {

        public Guid guid = Guid.NewGuid();
    }

    public class ServicioSingleton
    {

        public Guid guid = Guid.NewGuid();
    }

}
