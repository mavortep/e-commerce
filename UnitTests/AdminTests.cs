using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyFirstWebApplication.Controllers;
using MyFirstWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UnitTests
{
	[TestClass]
	public class AdminTests
	{
		[TestMethod]
		public void Index_Contains_All_Dishes()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1"},
				new Dish{DishId = 2, Style = "Style2"},
				new Dish{DishId = 3, Style = "Style3"},
				new Dish{DishId = 4, Style = "Style4"},
				new Dish{DishId = 5, Style = "Style5"}
			});

			AdminController controller = new AdminController(mock.Object);

			List<Dish> result = ((IEnumerable<Dish>)controller.Index().ViewData.Model).ToList();

			Assert.AreEqual(result.Count(), 5);
			Assert.AreEqual(result[0].Style, "Style1");
			Assert.AreEqual(result[1].Style, "Style2");
		}

		[TestMethod]
		public void Can_Edit_Dish()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1"},
				new Dish{DishId = 2, Style = "Style2"},
				new Dish{DishId = 3, Style = "Style3"},
				new Dish{DishId = 4, Style = "Style4"},
				new Dish{DishId = 5, Style = "Style5"}
			});

			AdminController controller = new AdminController(mock.Object);

			Dish dish1 = controller.Edit(1).ViewData.Model as Dish;
			Dish dish2 = controller.Edit(2).ViewData.Model as Dish;
			Dish dish3 = controller.Edit(3).ViewData.Model as Dish;

			Assert.AreEqual(1, dish1.DishId);
			Assert.AreEqual(2, dish2.DishId);
			Assert.AreEqual(3, dish3.DishId);
		}

		[TestMethod]
		public void Cannot_Edit_Nonexistent_Dish()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1"},
				new Dish{DishId = 2, Style = "Style2"},
				new Dish{DishId = 3, Style = "Style3"},
				new Dish{DishId = 4, Style = "Style4"},
				new Dish{DishId = 5, Style = "Style5"}
			});

			AdminController controller = new AdminController(mock.Object);

			Dish result = controller.Edit(7).ViewData.Model as Dish;

			Assert.IsNull(result);
		}

		[TestMethod]
		public void Can_Save_Valid_Changes()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			AdminController controller = new AdminController(mock.Object);

			Dish dish = new Dish { Style = "Test" };

			ActionResult result = controller.Edit(dish);

			mock.Verify(m => m.SaveDish(dish));

			Assert.IsNotInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public void Can_Save_Invalid_Changes()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			AdminController controller = new AdminController(mock.Object);

			Dish dish = new Dish { Style = "Test" };

			controller.ModelState.AddModelError("error", "error");

			ActionResult result = controller.Edit(dish);

			mock.Verify(m => m.SaveDish(It.IsAny<Dish>()), Times.Never());

			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}
	}
}
