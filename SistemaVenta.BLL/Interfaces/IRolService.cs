using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IRolService
    {
        Task<List<Rol>> Lista();
    }
}