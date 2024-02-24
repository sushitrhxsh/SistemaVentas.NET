using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.AplicacionWeb.Utilidades
{
    public class GenericResponse<TObject>
    {
        public bool Estado { get; set; }
        public string? Mensaje { get; set; }
        public TObject? Objeto { get; set; }
        public List<TObject>? ListaObjeto { get; set; }       
    }
}