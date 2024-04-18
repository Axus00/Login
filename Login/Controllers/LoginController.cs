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
            
            Hour myHours = new()
            {
                EntryDate = DateTime.Now,
                OutDate = null,
                EmployeeId = Convert.ToInt32(usuario.Id)

            };

            _context.Hours.Add(myHours);
             _context.SaveChanges();
            
            Console.WriteLine($"AQUI HAY ALGO : {usuario.Id}");
            HttpContext.Response.Cookies.Append("Enter", Convert.ToString(usuario.Id));

            ViewData["Log"] = usuario.Id;
            
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
        var cookiesOut =  HttpContext.Request.Cookies["Enter"];

        int logout = Convert.ToInt32(ViewData["Log"]);
        Console.WriteLine($"ESCUHEMEEEEEEE :D : :D :  {ViewData["Log"]}");
        var historial = await _context.Hours.FirstOrDefaultAsync(h => h.Id == logout);
        Console.WriteLine($"Aqui va el resultado del historia: {historial}");
        historial.OutDate = DateTime.Now;
        
        
        _context.SaveChanges();
        HttpContext.Response.Cookies.Delete("Enter");
        HttpContext.Response.Cookies.Delete("UserAuth");
        
        return RedirectToAction("Index", "Login");
    }
}
