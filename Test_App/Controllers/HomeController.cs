using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using Test_App.Models;

namespace Test_App.Controllers
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
            return View();
        }
        [HttpPost]
        public IActionResult Index(UserBindingModel model)
        {
            ViewBag.result = IsUserExists(model.UserName);
            return View("Result");
        }

        public IActionResult Result()
        {
            return View("Index");
        }
        public string IsUserExists(string eUser) // Метод проверки введенного пользователя на наличие в списке пользователей Windows
        {
            var usersSearcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_UserAccount");
            var tmpUsers = usersSearcher.Get();
            List<string> users = new List<string>();
            foreach (var user in tmpUsers)
            {
                string result = user.ToString().Remove(0, user.ToString().IndexOf(',') + 7).Trim('"').Trim('/');
                users.Add(result);
            }
            if (!String.IsNullOrEmpty(eUser))
            {
                if (users.Contains(eUser))
                {
                    return $"Пользователь {eUser} есть";
                }
                else
                {
                    return $"Пользователя {eUser} нет";
                }
            }
            else
            {
                return "Пользователь не был введен";
            }
        }
    }
}
