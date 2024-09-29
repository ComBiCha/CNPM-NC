using E_commerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _dataContext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
			ViewBag.Keyword = searchTerm;

			return View(products);
		}
		public async Task<IActionResult> Details(long Id)
		{
			if (Id == null) return RedirectToAction("Index");

			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			//related
			var relatedProducts = await _dataContext.Products.Where(p => p.CategoryId == productsById.CategoryId && p.Id != productsById.Id)
				.Take(4)
				.ToListAsync();
			ViewBag.RelatedProducts = relatedProducts;

			return View(productsById);
		}
	}
}
