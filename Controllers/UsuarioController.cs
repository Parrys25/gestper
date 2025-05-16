using Microsoft.AspNetCore.Mvc;
using Gestper.Models;
using Gestper.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Gestper.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuario/Registrar
        public IActionResult Registrar()
        {
            return View("Views/registro/registro.layout.cshtml");
        }

        // POST: Usuario/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.IdRol = 3; // Asignamos el rol de usuario normal
                usuario.Activo = true; // Marcamos al usuario como activo

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View("Views/registro/registro.layout.cshtml");
        }

        // GET: Usuario/Login
        public IActionResult Login()
        {
            return View("Views/login/login.layout.cshtml");
        }

        // POST: Usuario/Login con LoginViewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == model.Correo);

                if (usuario != null)
                {
                    if (usuario.Contrasena == model.Contrasena)
                    {
                        HttpContext.Session.SetString("UsuarioCorreo", usuario.Correo);
                        HttpContext.Session.SetString("UsuarioId", usuario.IdUsuario.ToString());

                        return RedirectToAction("Index", "CRUD");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Contraseña incorrecta.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }
            }

            return View("Views/login/login.layout.cshtml", model);
        }

        [HttpPost]
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }

        // ✅ NUEVA ACCIÓN: Ver Tickets Asignados al Usuario
        public async Task<IActionResult> VerTickets(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.TicketsAsignados = await _context.Tickets
                .Where(t => t.IdSoporteAsignado == id)
                .ToListAsync();

            return View("Views/Usuario/VerTickets.cshtml", usuario);
        }

        // ✅ NUEVA ACCIÓN: Mostrar lista de usuarios (trabajadores)
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View("Views/Usuario/Index.cshtml", usuarios);
        }
    }
}

