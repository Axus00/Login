﻿using Login.Data;
using Login.Helpers;
using Login.Models;
using Login.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Login.Clases;


namespace Login.Controllers;


[Authorize(Roles = "Admin")]
public class CreateController : Controller
{
    private readonly BaseContext _context;

    private readonly HelperUploadFiles _helperUploadFiles;

    public CreateController(BaseContext context, HelperUploadFiles helperUploadFiles)
    {
        _context = context;
        _helperUploadFiles = helperUploadFiles;
    }
    
    
    //vista
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }
    
    

    [HttpPost]
    public async Task<IActionResult> Index(Employee employee, IFormFile archivo, int ubicacion, string password)
    {
        string nombreArchivo = archivo.FileName;
        string path = "";
        
        //cifrado de contraseña
        //string passwordCifrado = CifrarPassword(password, 4);
        string passwordCifrado = EncryptPassword.CifrarPassword(password, 3);
        
        switch (ubicacion)
        {
            case 0:
                path = await _helperUploadFiles.UploadFileAsync(archivo, nombreArchivo, Folders.Images);
                break;
            case 1:
                path = await _helperUploadFiles.UploadFileAsync(archivo, nombreArchivo, Folders.Documents);
                break;
            case 2:
                path = await _helperUploadFiles.UploadFileAsync(archivo, nombreArchivo, Folders.Uploads);
                break;
            case 3:
                path = await _helperUploadFiles.UploadFileAsync(archivo, nombreArchivo, Folders.Temp);
                break;
        }

        employee.Photo = nombreArchivo;
        employee.Password = passwordCifrado;
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        ViewBag.Message = "Se ha creado de forma exitosa";
        return RedirectToAction("Index");
    }
}