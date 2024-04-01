var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Obtener un elemento de la configuracion
var ambiente = builder.Configuration.GetValue<string>("ambiente");

app.MapGet("/", () => "Ambiente actual: " + ambiente);

app.Run();
