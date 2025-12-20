using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AlquilerAuto.Servicio;
using AlquilerAuto.Models;
using Microsoft.Identity.Client;

namespace AlquilerAuto.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
           Usuario usuario = _usuarioService.ValidarLogin(correo, clave);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Correo o clave incorrectos";
                return View();
            }
          }

            public async Task<IActionResult> Salir() 
            {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
             }
        }
    }

