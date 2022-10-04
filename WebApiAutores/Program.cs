using WebApiAutores;

var builder = WebApplication.CreateBuilder(args);

//==============agregar esto 
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

//==================
var app = builder.Build();

//configuracion del midleware
var servicioLogger=(ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

//========agregaresto 

startup.Configure(app,app.Environment,servicioLogger);

//==============
app.Run();
