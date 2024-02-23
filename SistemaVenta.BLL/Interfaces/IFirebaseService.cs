using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> SubirStore(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo);
        Task<bool> EliminarStore(string CarpetaDestino, string NombreArchivo);
    }
}