using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IUtilidadesService
    {
        string GenerarClave();
        string ConvertirSha256(string texto);
    }
}