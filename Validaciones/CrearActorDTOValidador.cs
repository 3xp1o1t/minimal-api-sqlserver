using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MinimalAPICurso.DTOs;

namespace MinimalAPICurso.Validaciones
{
    public class CrearActorDTOValidador : AbstractValidator<CrearActorDTO>
    {
        public CrearActorDTOValidador()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
            .MaximumLength(150).WithMessage(UtilidadesValidacion.MaximumLengthMensaje);

            var fechaMinima = new DateTime(1900, 1, 1);

            RuleFor(x => x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima)
            .WithMessage(UtilidadesValidacion.GreaterThanOrEqualToMensaje(fechaMinima));

        }
    }
}