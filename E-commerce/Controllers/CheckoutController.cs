using E_commerce.Models;
using E_commerce.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _datacontext;
		public CheckoutController(DataContext context)
		{
			_datacontext = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString(); //123
				var orderItem = new OrderModel();
				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_datacontext.Add(orderItem);
				_datacontext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					_datacontext.Add(orderdetails);
					_datacontext.SaveChanges();

				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Checkout successfully";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}
	}
}
