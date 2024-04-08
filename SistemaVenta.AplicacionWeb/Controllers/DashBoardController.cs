using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {

        private readonly IDashBoardService _dashboardService;
        public DashBoardController(IDashBoardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            GenericResponse<VMDashBoard> gResponse = new GenericResponse<VMDashBoard>();
            try
            {
                VMDashBoard vmDashBoard = new VMDashBoard();
                vmDashBoard.TotalVentas = await _dashboardService.TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await _dashboardService.TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await _dashboardService.TotalProductos();
                vmDashBoard.TotalCategorias = await _dashboardService.TotalCategorias();

                List<VMVentasSemana> listaVentasSemana = new List<VMVentasSemana>();
                List<VMProductosSemana> listaProductoSemana = new List<VMProductosSemana>();

                foreach(KeyValuePair<string,int> item in await _dashboardService.VentasUltimaSemana()){
                    listaVentasSemana.Add(new VMVentasSemana()
                    { 
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                foreach(KeyValuePair<string,int> item in await _dashboardService.ProductosTopUltimaSemana()){
                    listaProductoSemana.Add(new VMProductosSemana()
                    { 
                        Producto = item.Key,
                        Cantidad = item.Value
                    });
                }
                
                vmDashBoard.VentasUltimaSemana = listaVentasSemana;
                vmDashBoard.ProductosTopUltimaSemana = listaProductoSemana;

                gResponse.Estado = true;
                gResponse.Objeto = vmDashBoard;

            } catch(Exception ex) {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK,gResponse);
        }

    }
}
