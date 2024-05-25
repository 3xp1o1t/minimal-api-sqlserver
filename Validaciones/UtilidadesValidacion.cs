using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPICurso.Validaciones
{
    public static class UtilidadesValidacion
    {
        public static string CampoRequeridoMensaje = "El campo {PropertyName} es requerido";
        public static string MaximumLengthMensaje = "El campo {PropertyName} debe tener menos de {MaxLength} caracteres";
        public static string PrimeraLetraEnMayusculasMensaje = "El campo {PropertyName} debe comenzar con mayusculas";

        public static string GreaterThanOrEqualToMensaje(DateTime fechaMinima)
        {
            return "El campo {PropertyName} debe ser posterior a " + fechaMinima.ToString("yyyy-MM-dd");
        }
        public static bool PrimeraLetraEnMayusculas(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return true;
            }

            var primeraLetra = valor[0].ToString();

            return primeraLetra == primeraLetra.ToUpper();
        }
    }
}