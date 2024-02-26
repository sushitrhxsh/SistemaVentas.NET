using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMPDFVenta
    {
        public VMNegocio? negocio { get; set; }
        public VMVenta? venta { get; set; }
    }
}