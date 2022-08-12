using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // needed for .Include()
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class CategoryController : Controller
{
    // the following context things are needed to inject the context service into the controller
    private MyContext _context;

    public CategoryController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("categories")]
    public IActionResult CategoryDisplay()
    {
        List<Category> AllCategories = _context.Categories.ToList();
        return View("CatFormDisplay", AllCategories);
    }

    [HttpPost("create/category")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if(ModelState.IsValid == false)
        {
            return CategoryDisplay();
        }
        _context.Categories.Add(newCategory);
        _context.SaveChanges();
        return RedirectToAction("CategoryDisplay");
    }

    [HttpGet("categories/{CategoryId}")]
    public IActionResult CategoryProducts(int CategoryId)
    {
        ViewBag.ProductsWithoutThisCategory = _context.Products.Include(product => product.Cats).Where(product => !product.Cats.Any(cat => cat.CategoryId == CategoryId));

        ViewBag.Cat = _context.Categories.Include(cat => cat.Prods).ThenInclude(assoc => assoc.Product).FirstOrDefault(cat => cat.CategoryId == CategoryId);
        return View("CatWithProdFormDisplay");
    }

    [HttpPost("create/catwithprod")]
    public IActionResult CreateCategoryWithProducts(Association newAssociation, int CategoryId) 
    {
        // have to assign the category id to the newAssociation
        newAssociation.CategoryId = CategoryId;
        
        if(ModelState.IsValid == false)
        {
            return CategoryProducts(CategoryId);
        }
        _context.Associations.Add(newAssociation);
        _context.SaveChanges();
        return RedirectToAction("CategoryProducts", new {CategoryId = CategoryId});
    }
}