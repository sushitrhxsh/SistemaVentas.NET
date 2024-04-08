using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;
using Microsoft.AspNetCore.Authorization;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {   

        private readonly IMapper _mapper;
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(IMapper mapper, ICategoriaService categoriaService)
        {
            _mapper = mapper;
            _categoriaService = categoriaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMCategoria> vmCategoriaLista = _mapper.Map<List<VMCategoria>>(await _categoriaService.Lista());
            return StatusCode(StatusCodes.Status200OK, new {data = vmCategoriaLista});
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody]VMCategoria modelo)
        {
            GenericResponse<VMCategoria> gResponse = new GenericResponse<VMCategoria>();

            try
            {
                Categoria categoria_creada = await _categoriaService.Crear(_mapper.Map<Categoria>(modelo));
                modelo = _mapper.Map<VMCategoria>(categoria_creada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;

            } catch(Exception ex) {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody]VMCategoria modelo)
        {
            GenericResponse<VMCategoria> gResponse = new GenericResponse<VMCategoria>();

            try
            {
                Categoria categoria_editada = await _categoriaService.Editar(_mapper.Map<Categoria>(modelo));
                modelo = _mapper.Map<VMCategoria>(categoria_editada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;

            } catch(Exception ex) {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdCategoria)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _categoriaService.Eliminar(IdCategoria);
            
            } catch(Exception ex) {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

    }
}
