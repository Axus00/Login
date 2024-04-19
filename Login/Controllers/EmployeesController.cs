using Login.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Login.Controllers;

[Authorize(Roles = "Admin, Employee")]
public class EmployeesController : Controller
{
    private readonly BaseContext _context;

    public EmployeesController(BaseContext context)
    {
        _context = context;
    }
    
    //vistas
    public async Task<IActionResult>  Index()
    {
        return View(await _context.Employees.ToListAsync());
    }
    
    [Authorize(Roles = "Admin")]
    //Delete
    public async Task<IActionResult> Delete(int? id)
    {
        var employee =await  _context.Employees.FindAsync(id);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    
    //Details
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> Details(int? id)
    {
        return View(await _context.Employees.FirstOrDefaultAsync(e => e.Id == id));
    }
    
    //Search
    [HttpGet]
    public async Task<IActionResult> Search(string search)
    {
        var employee = _context.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            employee = employee.Where(e => e.Names.Contains(search) || e.Id.ToString().Contains(search));
        }
        
        return View("Index", employee.ToList());
    }
}