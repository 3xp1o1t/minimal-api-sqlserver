using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPICurso.Endpoints;
using MinimalAPICurso.Entidades;
using MinimalAPICurso.Repositorios;
using MinimalAPICurso.Servicios;

var builder = WebApplication.CreateBuilder(args);
// Obtener un elemento de la configuracion
var ambiente = builder.Configuration.GetValue<string>("Ambiente");
var origenes = builder.Configuration.GetValue<string>("OrigenesPermitidos")!;
// Inicia area de Servicios

// CORS es un servicio
builder.Services.AddCors(options =>
{
  // Politica por Defecto
  options.AddDefaultPolicy(configuration =>
  {
    configuration.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod();
  });

  // Politica personalizada
  options.AddPolicy("any", configuration =>
  {
    configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

  });
});

// Servicio de cache
builder.Services.AddOutputCache();

// Habilitar servicio de swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyectar dependencias
builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();

// Servicio de subida de archivos Azure
//builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();

// Servicio de subida de archivo en Local, requiere el servicio de HttpContextAccessor
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();



// AutoMapper - typeof permite buscar recursos desde la raiz.
builder.Services.AddAutoMapper(typeof(Program));

// Termina area de Servicios

var app = builder.Build();

// Inicia area de Middlewares

// Habilitar el middleware de Swagger antes de los cors
// Es posible habilitar solo swagger cuando estemos desarrollando
if (builder.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Requerio para la carga de archivos estaticos
app.UseStaticFiles();

// Habilitar el middleware de CORS
app.UseCors();

// Habilitar el middleware de cache
app.UseOutputCache();

// Este endpoint utiliza cualquier origen, cualquier cabecera y cualquier metodo
app.MapGet("/", [EnableCors(policyName: "any")] () => "Ambiente actual: " + ambiente);

// Map Group para agrupar los Endpoints
app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();

// Termina area de Middlewares

app.Run();
