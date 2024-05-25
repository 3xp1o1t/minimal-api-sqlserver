using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Repositorios;

namespace MinimalAPICurso.Validaciones
{
    public class CrearGeneroDTOValidador : AbstractValidator<CrearGeneroDTO>
    {
        public CrearGeneroDTOValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.Nombre).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
                .MaximumLength(50).WithMessage(UtilidadesValidacion.MaximumLengthMensaje)
                .Must(UtilidadesValidacion.PrimeraLetraEnMayusculas).WithMessage(UtilidadesValidacion.PrimeraLetraEnMayusculasMensaje)
                .MustAsync(async (nombre, _) =>
                {
                    var existe = await repositorioGeneros.ExisteGenero(id, nombre);
                    return !existe;
                }).WithMessage(g => $"Ya existe un genero con el nombre {g.Nombre}");
        }

    }
}