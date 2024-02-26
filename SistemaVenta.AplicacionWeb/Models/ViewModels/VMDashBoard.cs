using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMDashBoard
    {
        public int? TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int? TotalProductos { get; set; }
        public int? TotalCategorias { get; set; }
        public List<VMVentasSemana> VentasUltimaSemana { get; set; }
        public List<VMVentasSemana> VentasTopUltimaSemana { get; set; }
    }
}