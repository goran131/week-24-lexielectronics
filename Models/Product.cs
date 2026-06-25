using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace LexiElectronics.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? ArticleNo { get; set; }

        public string LongDescription { get; set; }

        public string ShortDescription { get; set; }

        public int Price { get; set; }
        
        public int? Stock { get; set; }
        
        [MaxLength(100)]
        public string? ImageFilename { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ManufacturerId { get; set; }
                    
        public bool VisibleInShop { get; set; } = true;

        [NotMapped]
        public ProductCategory? Category { get; set; }

        [NotMapped]
        public int Quantity { get; set; }

        [NotMapped]
        public int nbrOfItemsSold { get; set; }

        [NotMapped]
        public Manufacturer? Manufacturer { get; set; }
    }
}
