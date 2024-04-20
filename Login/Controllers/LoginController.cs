using System.Runtime.InteropServices.JavaScript;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;//se agrega librería de cookies
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Login.Clases;
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
    public async Task<IActionResult>  Login(string email, string password, Hour hora, Employee employee)
    {
        var usuario = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);

        
        var UserEntry = DateTime.Now.ToString();//variable global
        
        
        
        if (usuario != null)
        {
            hora = new Hour();
            hora.EntryDate = DateTime.Now;
            hora.EmployeeId = usuario.Id;
            
            _context.Hours.Add(hora);
            
            
            
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
            Console.WriteLine("estoy");
            return RedirectToAction("Index", "Home");
        }
        
        //Desencriptar password
        string passwordSave = employee.Password;

        string decryptPassword = EncryptPassword.DesencriptarPassword(passwordSave, 3);
        
        
        //check the password
        if (passwordSave == decryptPassword)
        {
            ViewBag.Message = "Login is already";
            Console.WriteLine("Se ingresó correctamente");
                
                
            return RedirectToAction("Index", "Home");
        }
        
        return RedirectToAction("Index", "Login");

    }
    
    //Logout Session
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
                    HttpContext.Response.Cookies.Delete("EmployeeId");
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
