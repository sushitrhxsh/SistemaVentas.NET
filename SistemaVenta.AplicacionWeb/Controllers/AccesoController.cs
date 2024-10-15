using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AutoMapper;
using Newtonsoft.Json;
using SistemaVenta.AplicacionWeb.Utilidades.Response;

using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication; 

namespace SistemaVenta.AplicacionWeb.Controllers
{
    /*[Route("[controller]")]*/
    public class AccesoController : Controller
    {

        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        public AccesoController(IUsuarioService usuarioService,IMapper mapper)
        {
            _usuarioService = usuarioService;  
            _mapper = mapper;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser.Identity.IsAuthenticated){
                return RedirectToAction("Index","Home");
            }

            return View();
        }

        public IActionResult RestablecerClave()
        {
            return View();
        }
        
        public IActionResult Registrar(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            Usuario usuario_encontrado =  await _usuarioService.ObtenerPorCredenciales(modelo.Correo, modelo.Clave);
            
            if(usuario_encontrado == null){
                ViewData["mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            ViewData["mensaje"] = null;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario_encontrado.IdRol.ToString()),
                new Claim("UrlFoto", usuario_encontrado.UrlFoto),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties(){
                AllowRefresh = true,
                IsPersistent = modelo.MantenerSesion 
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerClave(VMUsuarioLogin modelo)
        {
            try
            {
                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/RestablecerClave?clave=[clave]";
                bool resultado = await _usuarioService.RestablecerClave(modelo.Correo,urlPlantillaCorreo);

                if(resultado){
                    ViewData["Mensaje"] = "Listo su contraseña fue restablecida. Revise su correo.";
                    ViewData["MensajeError"] = null;
                } else {
                    ViewData["MensajeError"] = "Tenemos problemas. Porfavor intentelo de nuevo mas tarde.";
                    ViewData["Mensaje"] = null;
                }

            } catch(Exception ex) {
                ViewData["MensajeError"] = ex.Message;
                ViewData["Mensaje"] = null;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromForm] IFormFile foto,[FromForm] string modelo){
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                string nombreFoto = "";
                Stream fotoStream = null;

                if(foto != null){
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }

                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/EnviarClave?correo=[correo]&clave=[clave]";

                Usuario usuario_creado = await _usuarioService.Crear(_mapper.Map<Usuario>(vmUsuario),fotoStream,nombreFoto,urlPlantillaCorreo);
                vmUsuario = _mapper.Map<VMUsuario>(usuario_creado);
                
                gResponse.Estado = true;
                gResponse.Objeto = vmUsuario;

            } catch (Exception ex) {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

    }
}