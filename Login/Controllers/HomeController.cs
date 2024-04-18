using Login.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Login.Controllers;
using Login.Models;

namespace Login.Controllers;

public class HomeController : Controller
{
    private readonly BaseContext _context;

    public HomeController(BaseContext context)
    {
        _context = context;
    }
    

    public async Task<IActionResult> Index()
    {
        var cookie = HttpContext.Request.Cookies["UserAuth"];
        var horaEntrada = HttpContext.Request.Cookies["Hora"];

        if (horaEntrada != null)
        {
            @ViewBag.hora = horaEntrada;
        }
        @ViewBag.saveCookie = cookie;
        return View(await _context.Employees.ToListAsync());
    }

    //variable
    [HttpPost]
    public async Task<IActionResult>  Entry()
    {
        var cookie = HttpContext.Request.Cookies["UserAuth"];
        Hour myHours = new()
        {
            EntryDate = DateTime.Now,
            OutDate = null,
            EmployeeId = Convert.ToInt32(cookie)

        };
        Console.WriteLine($"AQUI HAY ALGO PUTA : {myHours.Id}");
        HttpContext.Response.Cookies.Append("Enter", Convert.ToString(myHours.Id));
        
        return View("Index");
    }
    

    

   
}