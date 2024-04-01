using MinimalAPICurso.Entidades;

var builder = WebApplication.CreateBuilder(args);
// Inicia area de Servicios

// Termina area de Servicios

var app = builder.Build();

// Inicia area de Middlewares

// Obtener un elemento de la configuracion
var ambiente = builder.Configuration.GetValue<string>("ambiente");

app.MapGet("/", () => "Ambiente actual: " + ambiente);

// Generos
app.MapGet("/generos", () => {

  var generos = new List<Genero>
  {
    new Genero { Id = 1, Nombre = "Accion" },
    new Genero { Id = 2, Nombre = "Comedia"},
    new Genero { Id = 3, Nombre = "Fantasia"}
  };

  return generos;
});

// Termina area de Middlewares

app.Run();
