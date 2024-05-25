using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
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
builder.Services.AddScoped<IRepositorioPeliculas, RepositorioPeliculas>();
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();
builder.Services.AddScoped<IRepositorioErrores, RepositorioErrores>();


// Servicio de subida de archivos Azure
//builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();

// Servicio de subida de archivo en Local, requiere el servicio de HttpContextAccessor
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

// AutoMapper - typeof permite buscar recursos desde la raiz.
builder.Services.AddAutoMapper(typeof(Program));

// Fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Agregar errores personalizados cuando hay excepciones no controladas
// Va de la mano con los middleware UseExceptionHandler y UseStatusCodePages
builder.Services.AddProblemDetails();

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

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
  var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

  var excepcion = exceptionHandlerFeature?.Error!;

  var error = new Error();
  error.Fecha = DateTime.UtcNow;
  error.MensajeDeError = excepcion.Message;
  error.StackTrace = excepcion.StackTrace;

  var repositorio = context.RequestServices.GetRequiredService<IRepositorioErrores>();

  await repositorio.CrearMensajeError(error);

  await Results.BadRequest(
    new { tipo = "error", mensaje = "ha ocurrido un mensaje de error inesperado", estatus = 500 }
  ).ExecuteAsync(context);
}));
app.UseStatusCodePages();

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
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();

// Termina area de Middlewares

app.Run();
