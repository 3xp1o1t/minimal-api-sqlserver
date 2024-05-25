using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MinimalAPICurso.DTOs;

namespace MinimalAPICurso.Validaciones
{
    public class CrearPeliculaDTOValidador : AbstractValidator<CrearPeliculaDTO>
    {
        public CrearPeliculaDTOValidador()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
            .MaximumLength(150).WithMessage(UtilidadesValidacion.MaximumLengthMensaje);
        }
    }
}