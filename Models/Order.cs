using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiElectronics.Data;

namespace LexiElectronics.Models
{
    public class Order
    {
        public int Id { get; set; }
       
        [MaxLength(450)]
        public string UserIdStr { get; set; }
        
        [NotMapped]
        public ApplicationUser User { get; set; }
        
        public DateTime OrderDate { get; set; }

        public int TotalSum { get; set; }

        public int FreightSum { get; set; }
        
        public int TotalSumToPay => TotalSum + FreightSum;

        [MaxLength(100)]
        public string Status { get; set; }

        [NotMapped]
        public List<OrderItem> OrderItems { get; set; }

        public Order() {}

    
        public void CalcTotalSum (){
            int totalSum = 0;
            foreach (OrderItem item  in OrderItems)
            {
                totalSum += item.Price * item.Quantity;
            }
            TotalSum = totalSum;
        }
    
    }
}
