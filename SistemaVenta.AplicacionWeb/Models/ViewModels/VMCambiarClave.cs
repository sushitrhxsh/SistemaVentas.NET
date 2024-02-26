using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMCambiarClave
    {
        public string? claveActual { get; set; }
        public string? claveNueva { get; set; }
    }
}