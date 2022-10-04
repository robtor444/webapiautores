using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using WebApiAutores.Controllers;
using WebApiAutores.Filtros;
using WebApiAutores.Midlewares;
using WebApiAutores.Servicios;
using WebApiAutores.Utilidades;

namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //para la relacion y que pueda acceder a los claim desde Comentarios
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            Configuration=configuration;
            
        }

        public IConfiguration Configuration { get; }


        //configuramos los servicos=
        //los servicios es la resolucion de una dependencia
        //configurada en el sistema de inyeccion de dependencias
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers
                (opciones =>
                {
                    // asi se agrega el filtro de manera global
                    opciones.Filters.Add(typeof(FiltroDeExcepcion));

                    //esto es para las versiones ******************************************
                    opciones.Conventions.Add(new SwaggerAgrupaPorVersion());

                })
                .AddJsonOptions(x =>
                //para omitir los cicles repetitivos 
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                //pata el http patch
                .AddNewtonsoftJson();

            //conexion
            //configura el appdbcontext como un servicio
            // es decir como el application db context a sido configurado como un servicio
            // cada vez que el applicaciontdbcontext aparesca como una dependencia de una clase
            // a travez del contructor, el sitema de inyeccion de dependencias se encargara de instancia
            //correctamente el appdbcontext con toda sus configuracioes
            // es decir cuando quiera en cualquier clase usar el appdbcontext usarlo, tndre que conlocarlo en el 
            //ontolador de la clase
            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            //Ejemplo de inyeccion de servicios
            //ejemplo de servicio transitorio
            //le digo que cuando una clase requier un IServicio se le de una instancia para la clase ServicioA
            //services.AddTransient<IServicio, ServicioA>();

            ////tipos de Servico================
            //AddTransient=Agregar un transitorio Es bueno para funciones que ejecutan
            //    algun tipo de funcionalidad y sin tener data de  transicion, es decir una funcion que hace algo y 
            //    retorna punto .
            //AddScoped= tendras instancias distintas, Por ejememp el AddDbContext es decir la misma peticion http 
            //    va tener acceso a la misma instancia del appdbcontext para trabajar con los mismos datos
            //AddSingleton= siempre tendremos la misma instancia del servicio, Si tenemos algun tipo de cache en memoria para 
            //    que podamos dar rapido al usuario, es decir la misma data compartida en cache con todos los usuarios
            ////===========================================
            //Ejemplos
            //services.AddTransient<ServicioTransient>();
            //services.AddScoped<ServicioScope>();
            //services.AddSingleton<ServicioSingleton>();

            // filtro s por ejecutarse es decir Filtros.MisFiltrosDeAccion
            services.AddTransient<MisFiltrosDeAccion>();
            //ya registrado el filtro puede usar en el controlador

            //SeConfigura el IHosterService
            services.AddHostedService<EscribirEnArchivo>();
            //luego creamos el carpeta wwwroot en proyecto


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            //agregamos el servicio de caching
            //services.AddResponseCaching();
            
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //verifica que haya firmado correctamente con la llave que esta en el appsetting.json
                .AddJwtBearer(opciones=>opciones.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer=false,
                    ValidateAudience=false,
                    //validar el tiempo de vida
                    ValidateLifetime=true,
                    //validar firma
                    ValidateIssuerSigningKey=true,
                    //configuramos la llave
                    IssuerSigningKey=new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                    ClockSkew=TimeSpan.Zero
                });

            services.AddSwaggerGen(
                

                //para mandar el token por swagger
                //mandar por el authorize de Swagger = Bearer CodigoTokenLogin
                c =>
                {
                    //********** versionamiento de api
                    //creo documentos de swaggr esto nos servira para las versiones que tengamos de nunestra api

                    c.SwaggerDoc("v1",new OpenApiInfo { Version="v1", Title = "WebApiAutores", Description="descripcion de v1"});
                    c.SwaggerDoc("v2",new OpenApiInfo { Version="v2",Title = "WebApiAutores", Description = "descripcion de v1" });

                  
                    //****************** versionamiento de api
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                }
               
                ) ;

            //automapper configuracion
            services.AddAutoMapper(typeof(Startup));




            //para agregar coors al istema=====================================

           

            
            
            
            services.AddCors(opciones =>
            {

                //politica por defecto
                opciones.AddDefaultPolicy(builder =>
                {
                    //significa las urls que van a tener acceso a la web api
                    //allow any method sigifica todos los metodos
                    //allow any header permite cuakquier caecera
                    builder.WithOrigins("https://www.apirequestff.io").AllowAnyMethod().AllowAnyHeader();
                });
            });
            //fin coors al istema=====================================

            //el identyty nos ayuda a proteger las acciones
            //esto es para Identity para el servicio de identity
            ///IdentityRole es para roles
            /// el password por defecto es min may numero y almenos un caracter especial
           
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ESTO ES PARA CONFIGURAR CLAIMS
            //CLAIMS ES UNA LLAVE A DICIONAL DEL WEB TOKEN 
            services.AddAuthorization(opciones =>
            {
                //agregamos politica de seguridad (nombre, funciion para ver en que conciste)
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
                opciones.AddPolicy("EsVendedor", politica => politica.RequireClaim("Vendedor"));
            });


            //servici de proteccion de datos encrioptacion con IDataPropteccionProvider============================//
            services.AddDataProtection();

        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {
            // app es midlewares

            //ejeml1 de midleware basico
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Estoy interceptando la tuberi ade midlewares");
            //});

            //ejemplo2 de condicionar un midleware con ruta== se hace con map
            //Es decir debe poner https://localhost:7093/ruta1
            //app.Map("/ruta1", app =>
            //{
            //    app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Estoy interceptando la tuberi ade midlewares");
            //    });

            //});

            //ejemplo 3
            // deseo tener todas las respuestas de las peticiones api y guardarlas en un logger
            // el use se usa para permitir el paso a otra midleware es decir no afecta a los otros procesos

            //app.Use(async (contexto, siguiente) =>
            //{
            //    //guardar en memoria 
            //    using(var ms=new MemoryStream())
            //    {
            //        var cuerpoOriginalRespuesta = contexto.Response.Body;
            //        contexto.Response.Body = ms;

            //        await siguiente.Invoke();

            //        ms.Seek(0, SeekOrigin.Begin);
            //        string respuesta = new StreamReader(ms).ReadToEnd();
            //        ms.Seek(0,SeekOrigin.Begin);

            //        await ms.CopyToAsync(cuerpoOriginalRespuesta);
            //        contexto.Response.Body = cuerpoOriginalRespuesta;

            //        //Luego ir a program y configurar el servicioLogger

            //        logger.LogInformation(respuesta);

            //    };
            //});

            //ejemplo4
            //Manera1
            // para usar el midleware como clase desde carpeta Midleware
            //app.UseMiddleware<LoggearRespuestaHttpMidleware>();

            //Manera2 desde una clase estatica
           app.UseLoggearRespuestaHttp();

            // Configure the HTTP request pipeline.
            //si estamos en dsarriollo agregamos estos midlewares
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();

            //******** para las versiones *****************************
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAutores v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutores v2");
                }
                );

            app.UseHttpsRedirection();

            //agregado nuevo 
            app.UseRouting();

            //Midleware del cache= se usara con un filtro
            //app.UseResponseCaching();


            //para los cors
            //app.UseCors();


            //primero son las autorizaciones
            app.UseAuthorization();

            //comentamos esto
            //app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private int LoggearRespuestaHttpMidleware()
        {
            throw new NotImplementedException();
        }
    }
}
