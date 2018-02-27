using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
	public class EFDishRepository : IDishRepository
	{
		EFDbContext context = new EFDbContext();
		public IEnumerable<Dish> Dishes
		{
			get { return context.Dishes; }
		}

		public void SaveDish(Dish dish)
		{
			if (dish.DishId == 0)
			{
				context.Dishes.Add(dish);
			}
			else
			{
				Dish dbEntry = context.Dishes.Find(dish.DishId);
				if (dbEntry != null)
				{
					dbEntry.Style = dish.Style;
					dbEntry.Producer = dish.Producer;
					dbEntry.Description = dish.Description;
					dbEntry.Type = dish.Type;
					dbEntry.Price = dish.Price;
					dbEntry.ImageUrl = dish.ImageUrl;
				}
			}
			context.SaveChanges();
		}
	}
}
