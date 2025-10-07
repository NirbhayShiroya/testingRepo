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
    public class AdminController : Controller
    {
        private readonly AdminServices _adminServices;
        public AdminController(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FetchAllEmployee()
        {
            List<EmployeeModel> employees = _adminServices.FetchAllEmployee();
            return View(employees);
        }


        public IActionResult AddNewEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewEmployee(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.ProfileImageFile != null && employee.ProfileImageFile.Length > 0)
                {
                    string directoryPath = Path.Combine("..", "MVC", "wwwroot", "profile_images");
                    var filename = employee.Email + Path.GetExtension(employee.ProfileImageFile.FileName);
                    var filepath = Path.Combine(directoryPath, filename);
                    Directory.CreateDirectory(directoryPath);

                    employee.ProfileImage = filename;
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        employee.ProfileImageFile.CopyTo(stream);
                    }
                }


                var status = _adminServices.AddEmployee(employee);
                if (status == 1)
                {
                    ViewData["message"] = "User Registred";
                    return RedirectToAction("FetchAllEmployee");
                }
                else if (status == 0)
                {
                    ViewData["message"] = "User Already Registred";
                }
                else
                {
                    ViewData["message"] = "There was some error while Registration";
                }
            }

            return View(employee);

        }

        public IActionResult UpdateEmployeeDetails(int id)
        {
            List<EmployeeModel> employees = _adminServices.FetchAllEmployee();
            EmployeeModel emp = employees.FirstOrDefault(i => i.Eid == id);
            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }

        [HttpPost]
        public IActionResult UpdateEmployeeDetails(EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (employee.ProfileImageFile != null && employee.ProfileImageFile.Length > 0)
            {
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_images");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filename = employee.Email + Path.GetExtension(employee.ProfileImageFile.FileName);
                var filepath = Path.Combine(directoryPath, filename);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    employee.ProfileImageFile.CopyTo(stream);
                }

                employee.ProfileImage = filename;
            }

            _adminServices.UpdateEmployeeDetails(employee);

            return RedirectToAction("FetchAllEmployee");
        }
    }
}