using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain
{
    public class BidListDTO
    {
        public int BidListId { get; set; }

        [Required(ErrorMessage ="Account is mandatory")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Type is mandatory")]
        public string Type { get; set; }

        public double BidQuantity { get; set; }
    }
}
