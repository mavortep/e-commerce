using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Cart
	{
		private List<CartLine> lineCollection = new List<CartLine>();

		public IEnumerable<CartLine> Lines { get { return lineCollection; } }

		public void AddItem(Dish dish, int quantity)
		{
			CartLine line = lineCollection
				.Where(d => d.Dish.DishId == dish.DishId)
				.FirstOrDefault();

			if (line == null)
			{
				lineCollection.Add(new CartLine { Dish = dish, Quantity = quantity });
			}
			else
			{
				line.Quantity += quantity;
			}
		}

		public void RemoveLine(Dish dish)
		{
			lineCollection.RemoveAll(l => l.Dish.DishId == dish.DishId);
		}

		public decimal ComputeTotalValue()
		{
			return lineCollection.Sum(e => e.Dish.Price * e.Quantity);
		}

		public void Clear()
		{
			lineCollection.Clear();
		}
	}


	public class CartLine
	{
		public Dish Dish { get; set; }
		public int Quantity { get; set; }
	}
}
