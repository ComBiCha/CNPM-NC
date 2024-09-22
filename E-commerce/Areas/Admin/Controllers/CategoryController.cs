using E_commerce.Models;
using E_commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {

            if (ModelState.IsValid)
            {

                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exist");
                    return View(category);
                }
                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Category added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Product removed successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category, int Id)
        {
            if (ModelState.IsValid)
            {

                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exist");
                    return View(category);
                }
                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }
    }
}
