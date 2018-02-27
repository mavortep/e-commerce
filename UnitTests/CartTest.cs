using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Domain.Abstract;
using MyFirstWebApplication.Controllers;
using System.Web.Mvc;
using MyFirstWebApplication.Models;

namespace UnitTests
{
	[TestClass]
	public class CartTest
	{
		[TestMethod]
		public void Can_Add_New_Lines()
		{
			Dish dish1 = new Dish { DishId = 1, Style = "Dish1" };
			Dish dish2 = new Dish { DishId = 2, Style = "Dish2" };

			Cart cart = new Cart();

			cart.AddItem(dish1, 1);
			cart.AddItem(dish2, 2);
			List<CartLine> results = cart.Lines.ToList();

			Assert.AreEqual(results.Count(), 2);
			Assert.AreEqual(results[0].Dish, dish1);
			Assert.AreEqual(results[1].Dish, dish2);
		}

		[TestMethod]
		public void Can_Add_Quantity_For_Existing_Lines()
		{
			Dish dish1 = new Dish { DishId = 1, Style = "Dish1" };
			Dish dish2 = new Dish { DishId = 2, Style = "Dish2" };

			Cart cart = new Cart();

			cart.AddItem(dish1, 1);
			cart.AddItem(dish2, 1);
			cart.AddItem(dish1, 5);
			List<CartLine> results = cart.Lines.OrderBy(c => c.Dish.DishId).ToList();

			Assert.AreEqual(results.Count(), 2);
			Assert.AreEqual(results[0].Quantity, 6);
			Assert.AreEqual(results[1].Quantity, 1);
		}


		[TestMethod]
		public void Can_Remove_Line()
		{
			Dish dish1 = new Dish { DishId = 1, Style = "Dish1" };
			Dish dish2 = new Dish { DishId = 2, Style = "Dish2" };
			Dish dish3 = new Dish { DishId = 3, Style = "Dish3" };

			Cart cart = new Cart();

			cart.AddItem(dish1, 1);
			cart.AddItem(dish2, 1);
			cart.AddItem(dish1, 5);
			cart.AddItem(dish3, 2);
			cart.RemoveLine(dish2);

			Assert.AreEqual(cart.Lines.Where(c => c.Dish == dish2).Count(), 0);
			Assert.AreEqual(cart.Lines.Count(), 2);
		}

		[TestMethod]
		public void Calculate_Cart_Total()
		{
			Dish dish1 = new Dish { DishId = 1, Style = "Dish1", Price = 100 };
			Dish dish2 = new Dish { DishId = 2, Style = "Dish2", Price = 55 };
			
			Cart cart = new Cart();

			cart.AddItem(dish1, 1);
			cart.AddItem(dish2, 1);
			cart.AddItem(dish1, 5);
			decimal result = cart.ComputeTotalValue();

			Assert.AreEqual(result, 655);
		}

		[TestMethod]
		public void Can_Clear_Contents()
		{
			Dish dish1 = new Dish { DishId = 1, Style = "Dish1", Price = 100 };
			Dish dish2 = new Dish { DishId = 2, Style = "Dish2", Price = 55 };

			Cart cart = new Cart();

			cart.AddItem(dish1, 1);
			cart.AddItem(dish2, 1);
			cart.AddItem(dish1, 5);
			cart.Clear();

			Assert.AreEqual(cart.Lines.Count(), 0);
		}

		[TestMethod]
		public void Can_Add_To_Cart()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>{
				new Dish { DishId = 1, Style = "Dish1", Type = "Type1"}
			}.AsQueryable());

			Cart cart = new Cart();

			CartController controller = new CartController(mock.Object, null);

			controller.AddToCart(cart, 1, null);

			Assert.AreEqual(cart.Lines.Count(), 1);
			Assert.AreEqual(cart.Lines.ToList()[0].Dish.DishId, 1);
		}

		[TestMethod]
		public void Adding_Dish_To_Cart_Goes_To_Cart_Screen()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>{
				new Dish { DishId = 1, Style = "Dish1", Type = "Type1"}
			}.AsQueryable());

			Cart cart = new Cart();

			CartController controller = new CartController(mock.Object, null);

			RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

			Assert.AreEqual(result.RouteValues["action"], "Index");
			Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
		}

		[TestMethod]
		public void Can_View_Cart_Contents()
		{
			Cart cart = new Cart();
			CartController target = new CartController(null, null);

			CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

			Assert.AreSame(result.Cart, cart);
			Assert.AreEqual(result.ReturnUrl, "myUrl");
		}

		[TestMethod]
		public void Cannot_Checkout_Empty_Cart()
		{
			Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
			Cart cart = new Cart();
			ShippingDetails shippingDetails = new ShippingDetails();

			CartController controller = new CartController(null, mock.Object);

			ViewResult result = controller.Checkout(cart, shippingDetails);

			mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void Cannot_Checkout_Invalid_ShippingDetails()
		{
			Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
			Cart cart = new Cart();
			cart.AddItem(new Dish(), 1);

			CartController controller = new CartController(null, mock.Object);
			controller.ModelState.AddModelError("error", "error");

			ViewResult result = controller.Checkout(cart, new ShippingDetails());

			mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

			Assert.AreEqual("", result.ViewName);
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void Cannot_Checkout_And_Submit_Order()
		{
			Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
			Cart cart = new Cart();
			cart.AddItem(new Dish(), 1);

			CartController controller = new CartController(null, mock.Object);
			
			ViewResult result = controller.Checkout(cart, new ShippingDetails());

			mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

			Assert.AreEqual("Completed", result.ViewName);
			Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
		}
	}
}
