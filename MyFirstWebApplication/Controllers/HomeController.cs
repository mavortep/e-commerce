using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApplication.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}

		[Authorize]
		public ActionResult About()
		{
			ViewBag.Message = "О нас";

			return View();
		}

		[Authorize]
		public ActionResult Contact()
		{
			ViewBag.Message = "Контакты";

			return View();
		}

		[Authorize]
		public ActionResult Copyright()
		{
			ViewBag.Message = "Об авторе";

			return View();
		}
	}
}