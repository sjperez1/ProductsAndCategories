using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // needed for .Include()
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class ProductController : Controller
{
    // the following context things are needed to inject the context service into the controller
    private MyContext _context;

    public ProductController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("products")]
    public IActionResult ProductDisplay()
    {
        List<Product> AllProducts = _context.Products.ToList();
        return View("FormDisplay", AllProducts);
    }

    [HttpPost("create/product")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if(ModelState.IsValid == false)
        {
            return ProductDisplay();
        }
        _context.Products.Add(newProduct);
        _context.SaveChanges();
        return RedirectToAction("ProductDisplay");
    }

    [HttpGet("products/{ProductId}")]
    public IActionResult ProductCategories(int ProductId)
    {
        ViewBag.CategoriesWithoutThisProduct = _context.Categories.Include(category => category.Prods).Where(category => !category.Prods.Any(prod => prod.ProductId == ProductId));

        ViewBag.Prod = _context.Products.Include(prod => prod.Cats).ThenInclude(assoc => assoc.Category).FirstOrDefault(prod => prod.ProductId == ProductId);
        return View("ProdWithCatFormDisplay");
    }

    [HttpPost("create/prodwithcat")]
    public IActionResult CreateProductsWithCategory(Association newAssociation, int ProductId) 
    {
        // have to assign the product id to the newAssociation
        newAssociation.ProductId = ProductId;
        
        if(ModelState.IsValid == false)
        {
            return ProductCategories(ProductId);
        }
        _context.Associations.Add(newAssociation);
        _context.SaveChanges();
        return RedirectToAction("ProductCategories", new {ProductId = ProductId});
    }
}