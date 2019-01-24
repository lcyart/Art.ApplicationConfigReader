using Art.ConfigurationReader.UI.Infrastructure.Repository;
using Art.ConfigurationReader.UI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Linq;

namespace Art.ConfigurationReader.UI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<ApplicationConfig> _appConfigRepository;
        public HomeController()
        {
            _appConfigRepository = new ApplicationConfigRepository();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_appConfigRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ApplicationConfig applicationConfig)
        {
            _appConfigRepository.Create(applicationConfig);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationConfig applicationConfig = _appConfigRepository.Detail(id.Value);
            if (applicationConfig == null)
            {
                return NotFound();
            }
            return View(applicationConfig);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationConfig applicationConfig = _appConfigRepository.Detail(id.Value);
            if (applicationConfig == null)
            {
                return NotFound();
            }
            return View(applicationConfig);
        }

        [HttpPost]
        public IActionResult Delete(ApplicationConfig applicationConfig)
        {
            var result = _appConfigRepository.Delete(applicationConfig.ID);
            if (result.IsAcknowledged == false)
            {
                return BadRequest("Unable to Delete ApplicationConfig " + applicationConfig.ID);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationConfig = _appConfigRepository.Detail(id.Value);
            if (applicationConfig == null)
            {
                return NotFound();
            }
            return View(applicationConfig);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationConfig applicationConfig)
        {
            var result = _appConfigRepository.Update(applicationConfig);
            if (result.IsAcknowledged == false)
            {
                return BadRequest("Unable to update ApplicationConfig  " + applicationConfig.Name);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
