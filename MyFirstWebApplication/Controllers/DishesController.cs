using Domain.Abstract;
using MyFirstWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApplication.Controllers
{
    public class DishesController : Controller
    {
		private IDishRepository repository;
		public int pageSize = 4;

		public DishesController(IDishRepository repo)
		{
			repository = repo;
		}

		public ViewResult List(string type, int page = 1)
		{
			DishListViewModel model = new DishListViewModel
			{
				Dishes = repository.Dishes
				.Where(d => type == null || d.Type == type)
				.OrderBy(dish => dish.DishId)
				.Skip((page - 1) * pageSize)
				.Take(pageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = pageSize,
					TotalItems = type == null ?
						repository.Dishes.Count() :
						repository.Dishes.Where(dish => dish.Type == type).Count()
				},
				CurrentType = type

			};

			return View(model);
		}
    }
}