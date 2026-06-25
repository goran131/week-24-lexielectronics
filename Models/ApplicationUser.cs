using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LexiElectronics.Models
{ 
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser                                   
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }
   
        public string InvoiceName { get; set; }

        public string InvoiceStreetAddress { get; set; }
        
        public string InvoiceZipcode { get; set; }

        public string InvoiceCity { get; set; }

        public string? DeliveryName { get; set; }

        public string? DeliveryStreetAddress { get; set; }

        public string? DeliveryZipcode { get; set; }

        public string? DeliveryCity { get; set; }

        [MaxLength(21)]
        public string Discriminator { get; set; }

        public bool PreventAccess { get; set; } = false;

        [NotMapped]
        public string? UserRoleId { get; set; }

        [NotMapped]
        public string? UserRoleName { get; set; }

        [NotMapped]
        public List<SelectListItem> UserRoleListItems { get; set; }


        public ApplicationUser(){ }
    }
}