using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cardstore_MVC.Models
{
    public class CardListing
    {
        [Key]
        public int CardNum { get; set; }
        [Required]
        public string ListingName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Price!")] 
        public int Price { get; set; }

        



    }
}
