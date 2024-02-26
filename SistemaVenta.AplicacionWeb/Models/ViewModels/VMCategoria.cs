using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMCategoria
    {
        public int IdCategoria { get; set; }
        public string? Descripcion { get; set; }
        public int? EsActivo { get; set; }
    }
}