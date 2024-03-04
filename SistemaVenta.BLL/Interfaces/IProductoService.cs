using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> Lista();
        Task<Producto> Crear(Producto entidad, Stream Imagen = null, string NombreImagen = "");
        Task<Producto> Editar(Producto entidad, Stream Imagen = null, string NombreImagen = "");
        Task<bool> Eliminar(int IdProducto);
    }
}