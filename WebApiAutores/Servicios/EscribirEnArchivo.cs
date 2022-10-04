namespace WebApiAutores.Servicios
{
    // escribira en un archivo cada 5 seg
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo1.txt";

        //para que escriba cada sierto tiempo
        //private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //cada 5 seg
            //timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso Iniciado");
            //para decir que la tarea a concluido
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            //detener tmer
            //timer.Dispose();
            Escribir("Proceso Finalizado");
            //para decir que la tarea a concluido
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecucion "+ DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
        }

        //metodo auxilia para escrivir en file
        private void Escribir(string mensaje)
        {
            var ruta = $@"{env.ContentRootPath}\wwwwroot\{nombreArchivo}";

            //append quiere decir que no crearemos un arhivo , sino que el file que ya existe
            //    se escribe linea  por linea
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
