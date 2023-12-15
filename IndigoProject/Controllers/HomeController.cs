using IndigoProject.DAL;
using IndigoProject.Models;
using IndigoProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IndigoProject.Controllers
{
    public class HomeController : Controller
    {

        AppDBC _db;
        public HomeController(AppDBC db)
        {
            _db = db;
        }


        public async  Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Posts= await _db.posts.ToListAsync()
            };

            return View(homeVM);
        }

    }
}