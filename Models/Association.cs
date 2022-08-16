#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategories.Models;

public class Association
{
    public int AssociationId {get;set;}

    [Display(Name = "Product")]
    public int ProductId {get;set;}

    [Display(Name = "Category")]
    public int CategoryId {get;set;}
    public Product? Product {get;set;}
    public Category? Category {get;set;}
}

