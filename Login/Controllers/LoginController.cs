﻿using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;//se agrega librería de cookies
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
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
    public async Task<IActionResult>  Login(string email, string password, Hour hora, Hour hourOut)
    {
        var usuario = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
        
        
        var UserEntry = DateTime.Now.ToString();//variable global
        var UserOut = DateTime.Now.ToString();

        hora.EntryDate = DateTime.Now;
        hora.EmployeeId = usuario.Id;

        hourOut.OutDate = DateTime.Now;
        hourOut.EmployeeId = usuario.Id;
        
        _context.Hours.Add(hora);
        _context.Hours.Update(hourOut);
        
        //Objeto para Employee
        Hour hour = new Hour()
        {
            EntryDate = DateTime.Now,
            OutDate = null
        };
        
        if (usuario != null)
        {
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
            HttpContext.Response.Cookies.Append("Hour", UserEntry, cookieOptions);
            HttpContext.Response.Cookies.Append("HourOut", UserOut, cookieOptions);
            
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
        
        //Limpiamos cookies
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        HttpContext.Response.Cookies.Delete("UserAuth");
        HttpContext.Response.Cookies.Delete("NameUser");
        HttpContext.Response.Cookies.Delete("Hour");
        HttpContext.Response.Cookies.Delete("HourOut");
        HttpContext.Session.Remove("sesion");
        
        //Borramos el caché de la sesión
        
        
        _context.SaveChanges();
        return RedirectToAction("Index", "Login");
    }
}
