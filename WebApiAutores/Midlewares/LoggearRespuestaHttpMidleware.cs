namespace WebApiAutores.Midlewares
{
    public static class LoggearRespuestaHttpMidlewareExtensions
    {
        public static IApplicationBuilder UseLoggearRespuestaHttp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggearRespuestaHttpMidleware>();
        }
    }
        public class LoggearRespuestaHttpMidleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoggearRespuestaHttpMidleware> logger;

        //a travez del request delegate podremos decir que queremos los siguientes midlewares
        public LoggearRespuestaHttpMidleware(RequestDelegate siguiente,ILogger<LoggearRespuestaHttpMidleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        //para usarlo como midleware esta clase debe tener un metodo Invoke o InvokeAsync
        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await siguiente(contexto);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                //Luego ir a program y configurar el servicioLogger

                logger.LogInformation(respuesta);

            };
        }
    }
}
