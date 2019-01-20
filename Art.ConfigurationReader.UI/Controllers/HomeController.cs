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
		private IMongoDatabase mongoDatabase;

		public IMongoDatabase GetMongoDatabase()
		{
			var mongoClient = new MongoClient("mongodb://localhost:27017");
			return mongoClient.GetDatabase("config");
		}

		[HttpGet]
		public IActionResult Index()
		{
			mongoDatabase = GetMongoDatabase();
			var result = mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").Find(FilterDefinition<ApplicationConfig>.Empty).ToList();
			return View(result);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ApplicationConfig applicationConfig)
		{
			try
			{
				mongoDatabase = GetMongoDatabase();
				mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").InsertOne(applicationConfig);
			}
			catch (Exception ex)
			{
				throw;
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			mongoDatabase = GetMongoDatabase();
            ApplicationConfig applicationConfig = mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").Find<ApplicationConfig>(k => k.ID == id).FirstOrDefault();
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
			mongoDatabase = GetMongoDatabase();
            ApplicationConfig applicationConfig = mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").Find<ApplicationConfig>(k => k.ID == id).FirstOrDefault();
			if (applicationConfig == null)
			{
				return NotFound();
			}
			return View(applicationConfig);
		}

		[HttpPost]
		public IActionResult Delete(ApplicationConfig applicationConfig)
		{
			try
			{
				mongoDatabase = GetMongoDatabase();
				var result = mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").DeleteOne<ApplicationConfig>(k => k.ID == applicationConfig.ID);
				if (result.IsAcknowledged == false)
				{
					return BadRequest("Unable to Delete ApplicationConfig " + applicationConfig.ID);
				}
			}
			catch (Exception ex)
			{
				throw;
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
			mongoDatabase = GetMongoDatabase();
			var applicationConfig = mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").Find<ApplicationConfig>(k => k.ID == id).FirstOrDefault();
			if (applicationConfig == null)
			{
				return NotFound();
			}
			return View(applicationConfig);
		}

		[HttpPost]
		public IActionResult Edit(ApplicationConfig applicationConfig)
		{
			try
			{
				mongoDatabase = GetMongoDatabase();
				var filter = Builders<ApplicationConfig>.Filter.Eq("ID", applicationConfig.ID);
				var updatestatement = Builders<ApplicationConfig>.Update.Set("ID", applicationConfig.ID);
				updatestatement = updatestatement.Set("Name", applicationConfig.Name);
				updatestatement = updatestatement.Set("Type", applicationConfig.Type);
                updatestatement = updatestatement.Set("Value", applicationConfig.Value);
                updatestatement = updatestatement.Set("IsActive", applicationConfig.IsActive);
                updatestatement = updatestatement.Set("ApplicationName", applicationConfig.ApplicationName);
                var result = mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").UpdateOne(filter, updatestatement);
				if (result.IsAcknowledged == false)
				{
					return BadRequest("Unable to update ApplicationConfig  " + applicationConfig.Name);
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return RedirectToAction("Index");
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
