# Minimal APIs con Dapper y Net 8 CURSO

## Crear el proyecto

Listar los templates disponibles

```bash
dotnet new list
```

Para crear el proyecto con el CLI

```bash
dotnet new web -o MinimalAPICurso
```

## Flujo del Proyecto

1. Se configura el proyecto desde el inicio con las librerias requeridas.
2. Se crea el Modelo (Entidad)
3. Se crea la tabla en SQLServer
4. Se crea el comportamiento IModelo (Interfaz)
5. Se crea el repositorio (Servicio que implementa el comportamiento)
6. Se crean los Store Procedures
7. Se agrega el servicio del IRepositorio, Repositorio en Program.cs
8. Se crea el DTO
9. Se mapea en (AutoMapperProfile)
10. Se crean los endpoints
11. Se mapea el grupo de endpoints nuevo en Program.cs middleware.

## Conceptos fundamentales de Net

1. **Solucion** e ws un conjunto de proyectos
2. **csproj** es un archivo similar a package.json
3. **launchSettings.json** es un archivo que solo es util en modo dev
4. **appsettings.json** este en conjunto con **development** son archivos donde se almacena configuracion la cual puede ser accedida desde nuestro proyecto, todo depende del entorno de trabajo (desa/prod)
5. **Servicios y middleware** una buena practica es acomodar correctamente nuestros servicios y middleware en appsettings
6. **Perdonar nulos** - Para perdonar un valor null cuando se declara en una variable y omitir la configuracion (<Nullable>enable</Nullable>) es posible usar **!** al final de null (string name {get;set;} = null!).
7. CORS es un Servicio, para permitir que todo se ejecute se puede hacer lo siguiente

   ```csharp
    // Program.cs
    builder.Services.AddCors(options =>
    options.AddDefaultPolicy(configuration =>
    {
      configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }));
   ```

8. **WithOrigins** permite definir una lista de origenes permitidos

```json
  "OrigenesPermitidos": "*",
```

9. **AddPolicy** se puede tener muchas politicas diferentes.
10. Es posible utilizar cualquier politica directamente en un endpoint

11. **Cache** Configurar el servicio de cache ayuda a mejorar el rendimiento de la aplicacion.

12. **MIDDLEWARE ORDEN** el orden de los middleware es importante, pues determina como seran ejecutados

13. **SCOPE_IDENTITY** obtener el ultimo ID generado/insertado.
14. **Nuget Packages** se requiere de Microsoft data SqlClient y Dapper.
15. **Endpoints** Los endpoints se pueden organizar en clases para tener un mejor control de los mismos.
16. **Inyeccion Dependencias** Esto permite que tu código dependa de abstracciones en lugar de implementaciones concretas
17. **AutoMapper** Auto mapper automatiza el proceso de los DTO's
18. **Instalar un paquede desde el CLI** dotnet add d:/DotNet-Apps/MinimalAPICurso/MinimalAPICurso.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection -v 12.0.1 -s https://api.nuget.org/v3/index.json
19. **@ antes de las cadenas** Basicamente ayudan al compilador a interpretar las cadenas como literales, es decir que se leen tal cual incluyendo caracteres especiales, ayuda a escaparlos.
20. **IFormFile** Representa cualquier tipo de archivo (PDF, Imagen, etc) normalmente enviado desde el cliente.
21. **Ignore** Se puede ignorar el mapeo de elementos entre DTO's y Modelos cuando se usan por ejemplo IFormFile para crear un modelo y string? para recibir una url del archivo creado.
22. **Anti-Forgery** se puede deshabilitar el chequeo de seguridad para la subida de archivos directamente en el endpoint con la opcion DisableAntiforgery().
23. **Comportamiento por default** En las nuevas versiones de .NET y c# se puede definir un comportamiento por defecto dentro de la misma interfaz como se hizo en IAlmacenarArchivos metodo Editar.
24. **Azure storage** portal.azure.com es un SaaS en linea para bases de datos, archivos, etc. (Requiere Azure.Storage.Blobs nuget)
25. **UseStaticFiles** middleware para permitirnos almacenar archivos localmente
