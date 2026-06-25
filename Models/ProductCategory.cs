using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LexiElectronics.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
       
        public string Description { get; set; }

        // [NotMapped]
        public List<Product> Products { get; set; } = new List<Product>();

        public ProductCategory() { }

        public ProductCategory(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
