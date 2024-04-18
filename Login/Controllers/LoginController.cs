using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Login.Controllers;

public class LoginController : Controller
{
    private readonly BaseContext _context;

    public LoginController(BaseContext context)
    {
        _context = context;
    }
    
    
    [Authorize]
    //vistas
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult>  Login(string email, string password)
    {
        var usuario = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);

        
        
        if (usuario != null)
        {
            Employee information = new()
            {
                Names = usuario.Names
            };
            
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

            ViewData["Log"] = usuario.Id;
            
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.ErrorMessage = "The information isn't available";
            return RedirectToAction("Index", "Login");
        }
        
        

    }
    
    public async Task<IActionResult> Logout()
    {
        
        HttpContext.Response.Cookies.Delete("UserAuth");
        HttpContext.Response.Cookies.Delete("NameUser");
        
        _context.SaveChanges();
        return RedirectToAction("Index", "Login");
    }
}
