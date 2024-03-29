using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public AccesoController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;      
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser.Identity.IsAuthenticated){
                return RedirectToAction("Index","Home");
            }

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

            List<Claim> claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario_encontrado.IdRol.ToString()),
                new Claim("UrlFoto", usuario_encontrado.UrlFoto),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties(){
                AllowRefresh = true,
                IsPersistent = modelo.MantenerSesion // Cambiar vmodels
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Index","Home");
        }

    }
}