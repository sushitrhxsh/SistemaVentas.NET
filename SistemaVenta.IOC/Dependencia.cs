using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.DAL.Implementacion;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.BLL.Implementacion;


namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependecia(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DBVENTAContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>(); // Servicios de Venta(Interface, Service)

            services.AddScoped<ICorreoService, CorreoService>();    // Servicios de Correo(Interface, Service)
            services.AddScoped<IFirebaseService, FirebaseService>(); // Servicios de Firebase(Interface, Service)
            
            services.AddScoped<IUtilidadesService, UtilidadesService>(); // Servicios de Utilidades(Interface, Service)
            services.AddScoped<IRolService, RolService>(); // Servicios de Rol(Interface, Service)
        
        }
    }
}
