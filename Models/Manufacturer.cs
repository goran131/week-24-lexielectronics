using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LexiElectronics.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string BrandName { get; set; }
        
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [NotMapped]
        public int? NbrOfProducts { get; set; }

        public Manufacturer() { }

        public Manufacturer(string brandName, string companyName) {
            this.BrandName = brandName;
            this.CompanyName = companyName;
        }
       
    }
}
