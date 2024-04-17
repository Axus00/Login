using Login.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Login.Controllers;

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
        @ViewBag.saveCookie = cookie;
        return View(await _context.Employees.ToListAsync());
    }


    

   
}