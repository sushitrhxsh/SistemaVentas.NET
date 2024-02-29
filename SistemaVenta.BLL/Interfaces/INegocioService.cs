using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface INegocioService
    {
        Task<Negocio> Obtener();
        Task<Negocio> GuardarCambios(Negocio entidad, Stream Logo=null, string NombreLogo="");     
    }
}