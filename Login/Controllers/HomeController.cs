using Login.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Login.Controllers;
using Login.Models;
using Microsoft.AspNetCore.Authorization;

namespace Login.Controllers;

[Authorize(Roles = "Employee, Admin")]
public class HomeController : Controller
{
    private readonly BaseContext _context;

    public HomeController(BaseContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var user = await _context.Hours.Where(e => e.EmployeeId.ToString() == HttpContext.Session.GetString("Sesion")).ToListAsync();
        //Recolectamos las Cookies
        var cookie = HttpContext.Request.Cookies["UserAuth"];
        var UserName = HttpContext.Request.Cookies["NameUser"];
        var Hour = HttpContext.Request.Cookies["Hour"];
        
        //obtenemos la sesión
        string nombreId = HttpContext.Session.GetString("Sesion");


        /*if (nombreId == null)
        {
            return RedirectToAction("Index", "Login");
        }*/
        
        @ViewBag.saveCookie = cookie;
        @ViewBag.User = UserName;
        @ViewBag.saveEntry = Hour;
        return View(user);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //variable
    
    
}