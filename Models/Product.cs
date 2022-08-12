#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ProductsAndCategories.Models;

public class Product
{
    [Key]
    public int ProductId {get; set;}
    [Required(ErrorMessage = "Name is required")]
    public string Name {get; set;}

    [Required(ErrorMessage = "Price is required")]
    public int? Price {get; set;}

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description:")]
    public string Description {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public DateTime UpdatedAt {get; set;} = DateTime.Now;

    public List<Association> Cats {get;set;} = new List<Association>();
}