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
    public async Task<IActionResult>  Entry(Employee hora)
    {
        
        var HoraEntrada = DateTime.Now.ToString();
        HttpContext.Response.Cookies.Append("Hora", HoraEntrada);
        _context.Employees.Add(hora);
        await _context.SaveChangesAsync();
        
        
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Out(Employee horaSalida)
    {
        var hora = DateTime.Now.ToString();
        HttpContext.Response.Cookies.Append("HoraSalida", hora);
        _context.Employees.Add(horaSalida);
        await _context.SaveChangesAsync();
        
        
        return View("Index");
    }

    

   
}