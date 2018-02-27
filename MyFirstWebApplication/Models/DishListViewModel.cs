using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyFirstWebApplication.Models;

namespace MyFirstWebApplication.Models
{
	public class DishListViewModel
	{
		public IEnumerable<Dish> Dishes { get; set; }
		public PagingInfo PagingInfo { get; set; }
		public string CurrentType { get; set; }
	}
}