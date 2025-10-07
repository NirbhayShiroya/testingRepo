using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Services;
using Repositories;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeServices _employeeServices;
        public EmployeeController(EmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestConnection()
        {
            _employeeServices.TestDatabaseConnection();
            return Content("Check console for database connection result.");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(vm_login login)
        {
            EmployeeModel employee = _employeeServices.Login(login);

            if (ModelState.IsValid)
            {
                Console.WriteLine(employee.Email);
            }
            return View(login);
        }

    }
}