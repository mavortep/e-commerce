using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using MyFirstWebApplication.Controllers;
using System.Linq;
using System.Web.Mvc;
using MyFirstWebApplication.Models;
using MyFirstWebApplication.HtmlHelpers;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Can_Paginate()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1" },
				new Dish{DishId = 2, Style = "Style2" },
				new Dish{DishId = 3, Style = "Style3" },
				new Dish{DishId = 4, Style = "Style4" },
				new Dish{DishId = 5, Style = "Style5" }
			});

			DishesController controller = new DishesController(mock.Object);
			controller.pageSize = 3;

			DishListViewModel result = (DishListViewModel)controller.List(null, 2).Model;

			List<Dish> dishes = result.Dishes.ToList();
			Assert.IsTrue(dishes.Count == 2);
			Assert.AreEqual(dishes[0].Style, "Style4");
			Assert.AreEqual(dishes[1].Style, "Style5");
		}

		[TestMethod]
		public void Can_Generate_Page_Links()
		{
			HtmlHelper myHelper = null;
			PagingInfo pagingInfo = new PagingInfo
			{
				CurrentPage = 2,
				TotalItems = 28,
				ItemsPerPage = 10
			};
			Func<int, string> pageUrlDelegate = i => "Page" + i;

			MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

			Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
				+ @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
				+ @"<a class=""btn btn-default"" href=""Page3"">3</a>",
				result.ToString());

		}

		[TestMethod]
		public void Can_Send_Pagination_View_Model()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1" },
				new Dish{DishId = 2, Style = "Style2" },
				new Dish{DishId = 3, Style = "Style3" },
				new Dish{DishId = 4, Style = "Style4" },
				new Dish{DishId = 5, Style = "Style5" }
			});

			DishesController controller = new DishesController(mock.Object);
			controller.pageSize = 3;

			DishListViewModel result = (DishListViewModel)controller.List(null, 2).Model;

			PagingInfo pagingInfo = result.PagingInfo;
			Assert.AreEqual(pagingInfo.CurrentPage, 2);
			Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
			Assert.AreEqual(pagingInfo.TotalItems, 5);
			Assert.AreEqual(pagingInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Can_Filter_Dishes()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1", Type = "Type1" },
				new Dish{DishId = 2, Style = "Style2", Type = "Type2" },
				new Dish{DishId = 3, Style = "Style3", Type = "Type1" },
				new Dish{DishId = 4, Style = "Style4", Type = "Type3" },
				new Dish{DishId = 5, Style = "Style5", Type = "Type2" }
			});

			DishesController controller = new DishesController(mock.Object);
			controller.pageSize = 3;

			List<Dish> result = ((DishListViewModel)controller.List("Type2", 1).Model).Dishes.ToList();

			Assert.AreEqual(result.Count(), 2);
			Assert.IsTrue(result[0].Style == "Style2" && result[0].Type == "Type2");
			Assert.IsTrue(result[1].Style == "Style5" && result[1].Type == "Type2");
		}

		[TestMethod]
		public void Can_Create_Categories()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1", Type = "Type1" },
				new Dish{DishId = 2, Style = "Style2", Type = "Type2" },
				new Dish{DishId = 3, Style = "Style3", Type = "Type1" },
				new Dish{DishId = 4, Style = "Style4", Type = "Type3" },
				new Dish{DishId = 5, Style = "Style5", Type = "Type2" }
			});

			NavController target = new NavController(mock.Object);

			List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

			Assert.AreEqual(result[0], "Type1");
			Assert.AreEqual(result[1], "Type2");
			Assert.AreEqual(result[2], "Type3");
		}

		[TestMethod]
		public void Indicates_Selected_Type()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1", Type = "Type1" },
				new Dish{DishId = 2, Style = "Style2", Type = "Type2" },
				new Dish{DishId = 3, Style = "Style3", Type = "Type1" },
				new Dish{DishId = 4, Style = "Style4", Type = "Type3" },
				new Dish{DishId = 5, Style = "Style5", Type = "Type2" }
			});

			NavController target = new NavController(mock.Object);

			string typeToSelect = "Type2";

			string result = target.Menu(typeToSelect).ViewBag.SelectedType;

			Assert.AreEqual(typeToSelect, result);
		}

		[TestMethod]
		public void Generate_Genre_Specific_Dish_Count()
		{
			Mock<IDishRepository> mock = new Mock<IDishRepository>();
			mock.Setup(m => m.Dishes).Returns(new List<Dish>
			{
				new Dish{DishId = 1, Style = "Style1", Type = "Type1" },
				new Dish{DishId = 2, Style = "Style2", Type = "Type2" },
				new Dish{DishId = 3, Style = "Style3", Type = "Type1" },
				new Dish{DishId = 4, Style = "Style4", Type = "Type3" },
				new Dish{DishId = 5, Style = "Style5", Type = "Type2" }
			});

			DishesController controller = new DishesController(mock.Object);
			controller.pageSize = 3;

			int res1 = ((DishListViewModel)controller.List("Type1").Model).PagingInfo.TotalItems;
			int res2 = ((DishListViewModel)controller.List("Type2").Model).PagingInfo.TotalItems;
			int res3 = ((DishListViewModel)controller.List("Type3").Model).PagingInfo.TotalItems;
			int resAll = ((DishListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

			Assert.AreEqual(res1, 2);
			Assert.AreEqual(res2, 2);
			Assert.AreEqual(res3, 1);
			Assert.AreEqual(resAll, 5);
		}
	}
}
