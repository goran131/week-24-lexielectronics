using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace LexiElectronics.Models.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? ArticleNo { get; set; }

        public string LongDescription { get; set; }

        [MaxLength(125)]
        public string ShortDescription { get; set; }

        public int Price { get; set; }
        
        public int? Stock { get; set; }
        
        [MaxLength(100)]
        public string? ImageFilename { get; set; }

        public IFormFile? ImageFile { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ManufacturerId { get; set; }

        public bool VisibleInShop { get; set; } = true;


        public List<SelectListItem> ProductCategoryListItems { get; set; }

        public List<SelectListItem> ManufacturerListItems { get; set; }

        public ProductVM() {}
    
        public ProductVM(List<SelectListItem> productCategoryListItem, List<SelectListItem> manufacturerListItems)
        {
            ProductCategoryListItems = productCategoryListItem;
            ManufacturerListItems = manufacturerListItems;
        }
        public ProductVM(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.ArticleNo = product.ArticleNo;
            this.ShortDescription = product.ShortDescription;
            this.LongDescription = product.LongDescription;
            this.Price = product.Price;
            this.Stock = product.Stock;
            this.ImageFilename = product.ImageFilename;
            this.ImageFile = null;
            this.ProductCategoryId = product.ProductCategoryId;
            this.ManufacturerId = product.ManufacturerId;
            this.VisibleInShop = product.VisibleInShop;
        }
    }
}
