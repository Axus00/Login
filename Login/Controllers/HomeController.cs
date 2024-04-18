using Login.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Login.Controllers;
using Login.Models;
using Microsoft.AspNetCore.Authorization;

namespace Login.Controllers;

[Authorize(Roles = "Employee")]
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
        var UserName = HttpContext.Request.Cookies["NameUser"];
        
        @ViewBag.saveCookie = cookie;
        @ViewBag.User = UserName;
        return View(await _context.Employees.ToListAsync());
    }

    //variable
    
    
}