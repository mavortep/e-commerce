using Domain.Abstract;
using Domain.Entities;
using MyFirstWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApplication.Controllers
{
    public class CartController : Controller
    {
		private IDishRepository repository;
		private IOrderProcessor orderProcessor;

		public CartController(IDishRepository repo, IOrderProcessor processor)
		{
			repository = repo;
			orderProcessor = processor;
		}

		public ViewResult Index(Cart cart, string returnUrl)
		{
			return View(new CartIndexViewModel
			{
				Cart = cart,
				ReturnUrl = returnUrl
			});
		}



		public RedirectToRouteResult AddToCart(Cart cart, int dishId, string returnUrl)
		{
			Dish dish = repository.Dishes
				.FirstOrDefault(d => d.DishId == dishId);

			if (dish != null)
			{
				cart.AddItem(dish, 1);
			}

			return RedirectToAction("Index", new { returnUrl });
		}

		public RedirectToRouteResult RemoveFromCart(Cart cart, int dishId, string returnUrl)
		{
			Dish dish = repository.Dishes
				.FirstOrDefault(d => d.DishId == dishId);

			if (dish != null)
			{
				cart.RemoveLine(dish);
			}

			return RedirectToAction("Index", new { returnUrl });
		}

		public PartialViewResult Summary(Cart cart)
		{
			return PartialView(cart);
		}

		public ViewResult Checkout()
		{
			return View(new ShippingDetails());
		}

		[HttpPost]
		public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
		{
			if (cart.Lines.Count() == 0)
			{
				ModelState.AddModelError("", "Извините, корзина пуста!");
			}

			if (ModelState.IsValid)
			{
				orderProcessor.ProcessOrder(cart, shippingDetails);
				cart.Clear();
				return View("Completed");
			}
			else
			{
				return View(new ShippingDetails());
			}			
		}
	}
}