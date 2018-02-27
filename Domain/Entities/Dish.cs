using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Entities
{
	public class Dish
	{
		[HiddenInput(DisplayValue=false)]
		[Display(Name = "ID")]
		public int DishId { get; set; } //BookId

		[Display(Name = "Название")]
		[Required(ErrorMessage = "Введите стиль")]
		public string Style { get; set; } //Name

		[Display(Name = "Производитель")]
		[Required(ErrorMessage = "Введите изготовителя")]
		public string Producer { get; set; } //Author

		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = "Введите описание товара")]
		[Display(Name = "Описание")]
		public string Description { get; set; } //Description

		[Display(Name = "Тип")]
		[Required(ErrorMessage = "Введите тип изделия")]
		public string Type { get; set; } //Genre

		[Display(Name = "Цена(грн)")]
		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное значение цены")]
		public decimal Price { get; set; } //Price

		[Display(Name = "Изображение")]
		[Required]
		public string ImageUrl { get; set; }
	}
}
