using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.Entity;
using AutoMapper;


namespace SistemaVenta.AplicacionWeb.Utilidades.Automapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {   
            #region Rol
            CreateMap<Rol,VMRol>()
                .ReverseMap();
            #endregion

            #region Usuario
            CreateMap<Usuario,VMUsuario>()
                .ForMember(dest => dest.EsActivo,   opt => opt.MapFrom(src => src.EsActivo == true?1:0))
                .ForMember(dest => dest.NombreRol,  opt => opt.MapFrom(src => src.IdRolNavigation.Descripcion));
            
            CreateMap<VMUsuario,Usuario>()
                .ForMember(dest => dest.EsActivo,   opt => opt.MapFrom(src => src.EsActivo == 1 ?true:false))
                .ForMember(dest => dest.IdRolNavigation, opt => opt.Ignore());
            #endregion
            
            #region Negocio
            CreateMap<Negocio,VMNegocio>()
                .ForMember(dest => dest.PorcentajeImpuesto, opt => opt.MapFrom(src => Convert.ToString(src.PorcentajeImpuesto.Value, new CultureInfo("es-MX"))));
            
            CreateMap<VMNegocio,Negocio>()
                .ForMember(dest => dest.PorcentajeImpuesto, opt => opt.MapFrom(src => Convert.ToDecimal(src.PorcentajeImpuesto, new CultureInfo("es-MX"))));
            
            #endregion

            #region Categoria
            CreateMap<Categoria,VMCategoria>()
                .ForMember(dest => dest.EsActivo,   opt => opt.MapFrom(src => src.EsActivo == true?1:0));
            
            CreateMap<VMCategoria,Categoria>()
                .ForMember(dest => dest.EsActivo,   opt => opt.MapFrom(src => src.EsActivo == 1?true:false));
            #endregion

            #region Producto
            CreateMap<Producto,VMProducto>()
                .ForMember(dest => dest.EsActivo,   opt => opt.MapFrom(src => src.EsActivo == true?1:0))
                .ForMember(dest => dest.NombreCategoria, opt => opt.MapFrom(src => src.IdCategoriaNavigation.Descripcion))
                .ForMember(dest => dest.Precio,     opt => opt.MapFrom(src => Convert.ToString(src.Precio.Value, new CultureInfo("es-MX"))));
            
           CreateMap<VMProducto,Producto>()
                .ForMember(dest => dest.EsActivo,   opt => opt.MapFrom(src => src.EsActivo == 1?true:false))
                .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Precio,     opt => opt.MapFrom(src => Convert.ToDecimal(src.Precio, new CultureInfo("es-MX"))));
            #endregion
            
            #region TipoDocumentoVenta
            CreateMap<TipoDocumentoVenta,VMTipoDocumentoVenta>()
                .ReverseMap();
            #endregion

            #region Venta
            CreateMap<Venta,VMVenta>()
                .ForMember(dest => dest.TipoDocumentoVenta, opt => opt.MapFrom(src => src.IdTipoDocumentoVentaNavigation.Descripcion))
                .ForMember(dest => dest.Usuario,        opt => opt.MapFrom(src => src.IdUsuarioNavigation.Nombre))
                .ForMember(dest => dest.SubTotal,       opt => opt.MapFrom(src => Convert.ToString(src.SubTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.ImpuestoTotal,  opt => opt.MapFrom(src => Convert.ToString(src.ImpuestoTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total,          opt => opt.MapFrom(src => Convert.ToString(src.Total.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.FechaRegistro,  opt => opt.MapFrom(src => src.FechaRegistro.Value.ToString("dd/MM/yyyy")));
            
            CreateMap<VMVenta,Venta>()
                .ForMember(dest => dest.SubTotal,       opt => opt.MapFrom(src => Convert.ToDecimal(src.SubTotal, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.ImpuestoTotal,  opt => opt.MapFrom(src => Convert.ToDecimal(src.ImpuestoTotal, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total,          opt => opt.MapFrom(src => Convert.ToDecimal(src.Total, new CultureInfo("es-MX"))));
            #endregion

            #region DetalleVenta
            CreateMap<DetalleVenta,VMDetalleVenta>()
                .ForMember(dest => dest.Precio,     opt => opt.MapFrom(src => Convert.ToString(src.Precio.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total,      opt => opt.MapFrom(src => Convert.ToString(src.Total.Value, new CultureInfo("es-MX"))));
            
            CreateMap<VMDetalleVenta,DetalleVenta>()
                .ForMember(dest => dest.Precio,     opt => opt.MapFrom(src => Convert.ToDecimal(src.Precio, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total,      opt => opt.MapFrom(src => Convert.ToDecimal(src.Total, new CultureInfo("es-MX"))));
            
            CreateMap<DetalleVenta,VMReporteVenta>()
                .ForMember(dest => dest.FechaRegistro,    opt => opt.MapFrom(src => src.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.NumeroVenta,      opt => opt.MapFrom(src => src.IdVentaNavigation.NumeroVenta))
                .ForMember(dest => dest.TipoDocumento,    opt => opt.MapFrom(src => src.IdVentaNavigation.IdTipoDocumentoVentaNavigation.Descripcion))
                .ForMember(dest => dest.DocumentoCliente, opt => opt.MapFrom(src => src.IdVentaNavigation.DocumentoCliente))
                .ForMember(dest => dest.NombreCliente,    opt => opt.MapFrom(src => src.IdVentaNavigation.NombreCliente))
                .ForMember(dest => dest.SubTotalVenta,    opt => opt.MapFrom(src => Convert.ToString(src.IdVentaNavigation.SubTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.ImpuestoTotalVenta, opt => opt.MapFrom(src => Convert.ToString(src.IdVentaNavigation.ImpuestoTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.TotalVenta,       opt => opt.MapFrom(src => Convert.ToString(src.IdVentaNavigation.Total.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Producto,         opt => opt.MapFrom(src => src.DescripcionProducto))
                .ForMember(dest => dest.Precio,           opt => opt.MapFrom(src => Convert.ToString(src.Precio.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total,            opt => opt.MapFrom(src => Convert.ToString(src.Total.Value, new CultureInfo("es-MX"))));
            #endregion

            #region Menu
            CreateMap<Menu,VMMenu>()
                .ForMember(dest => dest.SubMenus, opt => opt.MapFrom(src => src.InverseIdMenuPadreNavigation));
            #endregion

        }
    }
}