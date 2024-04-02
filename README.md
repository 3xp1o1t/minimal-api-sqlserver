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
