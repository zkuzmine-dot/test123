using Gorb.DAL.DB;
using Gorb.DAL.Entities;
using Gorb.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Gorb.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //using (var context = new ApplicationDbContext())
            //{
            //    // Создание базы данных и таблиц
            //    context.Database.EnsureCreated();


            //    var user = new User { Nickname = "John Doe" };


            //    context.Users.Add(user);


            //    context.SaveChanges();

            //    // Чтение данных
            //    var users = context.Users.ToList();


            //}
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
