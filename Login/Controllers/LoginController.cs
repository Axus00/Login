using System.Runtime.InteropServices.JavaScript;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;//se agrega librería de cookies
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace Login.Controllers;

public class LoginController : Controller
{
    private readonly BaseContext _context;
    

    public LoginController(BaseContext context)
    {
        _context = context;
        
    }
    
    
    //vistas
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult>  Login(string email, string password, Hour hora)
    {
        var usuario = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);

        
        
        var UserEntry = DateTime.Now.ToString();//variable global
        

        hora.EntryDate = DateTime.Now;
        hora.EmployeeId = usuario.Id;
        
        _context.Hours.Add(hora);

        Console.WriteLine("Se agregó la hora");
        
        
        if (usuario != null)
        {
            //configuramos roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Names),
                new Claim("Correo", usuario.Email)
            };
            foreach (string rol in usuario.Roles) //se definen roles de los usuarios
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            
            //Configuración de la session caché
            HttpContext.Session.SetString("Sesion", usuario.Id.ToString());
            
            
            //configuración de las cookies
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true
            };
            
            
            var id = usuario.Id.ToString();
            ViewBag.Message = "Login is already";
            
            
            HttpContext.Response.Cookies.Append("UserAuth", id, cookieOptions);
            HttpContext.Response.Cookies.Append("NameUser", usuario.Names, cookieOptions);
            HttpContext.Response.Cookies.Append("Hour", UserEntry, cookieOptions);
            HttpContext.Response.Cookies.Append("EmployeeId", usuario.Id.ToString(), cookieOptions);

            
            
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.ErrorMessage = "The information isn't available";
            return RedirectToAction("Index", "Login");
        }
        

    }
    
    public async Task<IActionResult> Logout(Hour exit)
    {
        

        if (HttpContext.Request.Cookies.TryGetValue("EmployeeId", out string employeeIdString))
        {
            if (int.TryParse(employeeIdString, out int employeeId))
            {
                Console.WriteLine(employeeId);
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
                
                if (employee != null)
                {
                    exit.OutDate = DateTime.Now;
                    exit.EmployeeId = employee.Id;

                
                
                    //Limpiamos cookies
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
                    HttpContext.Response.Cookies.Delete("UserAuth");
                    HttpContext.Response.Cookies.Delete("NameUser");
                    HttpContext.Response.Cookies.Delete("Hour");
                    HttpContext.Session.Remove("Sesion");
                    
                

                    _context.Update(exit);
                    await _context.SaveChangesAsync();
                    
                    
                    return RedirectToAction("Index", "Login");
                }
            }
        }

        ViewBag.ErrorMessage = "Employee not found";
        return RedirectToAction("Index", "Login");

    }
}
