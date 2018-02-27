using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApplication.Controllers
{
    public class AdminController : Controller
    {
		IDishRepository repository;

		public AdminController(IDishRepository repo)
		{
			repository = repo;
		}

		public ViewResult Index()
        {
            return View(repository.Dishes);
        }

		public ViewResult Edit(int dishId)
		{
			Dish dish = repository.Dishes.FirstOrDefault(d => d.DishId == dishId);
			return View(dish);
		}

		[HttpPost]
		public ActionResult Edit(Dish dish)
		{
			if (ModelState.IsValid)
			{
				repository.SaveDish(dish);
				TempData["message"] = string.Format("Изменения информации о товаре \"{0}\" сохранены", dish.Style);
				return RedirectToAction("Index");				
			}
			else
			{
				return View(dish);
			}
		}

		[HttpGet]
		public ActionResult SaveImages()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SaveImages(HttpPostedFileBase UploadedImage)
		{
			if (UploadedImage.ContentLength > 0)
			{
				string ImageFileName = Path.GetFileName(UploadedImage.FileName);

				string FolderPath = Path.Combine(Server.MapPath("~/UploadedImages"), ImageFileName);

				UploadedImage.SaveAs(FolderPath);
			}

			ViewBag.Message = "Image file uploaded successfully";

			return View();
		}
	}
}