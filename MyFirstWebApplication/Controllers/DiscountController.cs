using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebApplication.Controllers
{
    public class DiscountController : Controller
    {
		// GET: Discount
		[Authorize]
		public ActionResult DiscountGame()
		{
			ViewBag.Message = "Играй - скидку получай";

			return View();
		}
	}
}