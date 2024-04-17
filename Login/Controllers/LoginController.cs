using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
    public async Task<IActionResult>  Login(string email, string password)
    {
        var usuario =await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);

        if (usuario != null)
        {
            var id = usuario.Id.ToString();
            ViewBag.Message = "Login is already";
            HttpContext.Response.Cookies.Append("UserAuth", id);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.ErrorMessage = "The information isn't available";
            return RedirectToAction("Index", "Login");
        }

    }
    
    public IActionResult Logout()
    {
        
        HttpContext.Response.Cookies.Delete("UserAuth");
        return RedirectToAction("Index", "Login");
    }
}
