using E_commerce.Models;
using E_commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
			var DetailsOrder = await _dataContext.OrderDetails.Include(o=>o.Product).Where(o=>o.OrderCode==ordercode).ToListAsync();
            return View(DetailsOrder);
        }
        /*public async Task<IActionResult> Delete(int Id)
        {
            OrderController order = await _dataContext.Orders.FindAsync(Id);
            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Product removed successfully";
            return RedirectToAction("Index");
        }*/
    }
}
